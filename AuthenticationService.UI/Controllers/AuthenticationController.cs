using System;
using System.Net;
using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Services;
using AuthenticationService.UI.Configuration;
using AuthenticationService.UI.Services;
using AuthenticationService.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.UI.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region constants messages
        private const string DEFAULT_ERROR_MESSAGE = "Ocorreu um erro interno no servidor ";
        private const string INVALID_MODEL_MESSAGE = "O objeto recebido tem campos obrigatórios nulos";
        #endregion

        #region attributes
        private readonly TokenGeneratioService _tokenGenerator;
        private readonly UsuarioValidator _validator;
        private readonly IUsuarioService _usuarioService;
        #endregion 

        public AuthenticationController(
            IUsuarioService usuarioService, 
            UsuarioValidator validator, 
            TokenGeneratioService tokenGenerator)
        {
            _usuarioService = usuarioService;
            _validator = validator;
            _tokenGenerator = tokenGenerator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        public JsonResult SignUp(
            [FromBody] UsuarioDto usuarioRequisicao,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            Usuario usuario = UsuarioMapper(usuarioRequisicao);
            var results = _validator.Validate(usuario);

            if(results.IsValid){
                try{
                    //Atribui token
                    usuario.Token = GenerateToken(
                        usuario, 
                        signingConfigurations, 
                        tokenConfigurations);

                    //Atribui data de criação e ultimo logon (DateTime.Now)     
                    var preparedUser = _usuarioService.PrepareEntityToSave(usuario);

                    _usuarioService.Add(usuario);

                    return new JsonResult(usuario);
                }catch(Exception e)
                {
                    return new JsonResult(GetResponseErrorObj(
                        DEFAULT_ERROR_MESSAGE, (int)HttpStatusCode.InternalServerError));
                }
            }else{
                return new JsonResult(GetResponseErrorObj(
                    INVALID_MODEL_MESSAGE, (int)HttpStatusCode.BadRequest));
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromBody] CredentialsDto credentials)
        {
            return new JsonResult(credentials);
        }

        [Authorize("Bearer")]
        [HttpPost("{id}")]
        [Route("Profile")]
        public JsonResult Profile([FromQuery]string id, [FromHeader] string Bearer)
        {
            var internalId = Guid.Parse(id);
            var usuario = _usuarioService.Select(internalId);
            return new JsonResult(usuario);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [Route("Test")]
        public JsonResult TestGetUser([FromQuery]string id)
        {
            return new JsonResult(
                new ResponseMesageViewDto()
                { statusCode = (int)HttpStatusCode.Created, 
                mensagem = string.Format(DEFAULT_ERROR_MESSAGE)});
        }

        private Usuario UsuarioMapper(UsuarioDto usuarioRequisicao)
        {
            //==> SUBSTITUIR PELO USO DE AUTOMAPER===
            var usuario = new Usuario();
            usuario.Id = usuarioRequisicao.id;
            usuario.Nome = usuarioRequisicao.nome;
            usuario.Email = usuarioRequisicao.email;
            usuario.Senha = usuarioRequisicao.senha;
            if(usuarioRequisicao.telefones != null)
                usuario.Telefones.AddRange(usuarioRequisicao.telefones);

            return usuario;            
        }
        private object GetResponseErrorObj(string message, int statusCode)
        {
            return new ResponseMesageViewDto()
            { 
                statusCode = statusCode, 
                mensagem = string.Format(message)
            };
        }

        private string GenerateToken(
            Usuario usuario, 
            SigningConfigurations signingConfigurations, 
            TokenConfigurations tokenConfigurations)
        {
            return _tokenGenerator.GenerateToken(usuario, signingConfigurations, tokenConfigurations);
        }
    }
}
