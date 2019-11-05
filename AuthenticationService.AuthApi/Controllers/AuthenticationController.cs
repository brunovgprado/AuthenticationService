using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using AuthenticationService.Application.Service;
using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Services;
using AuthenticationService.AuthApi.ConfigurationsApi;
using AuthenticationService.AuthApi.Exceptions;
using AuthenticationService.AuthApi.Services;
using AuthenticationService.Application.AppModel.Dtos;

namespace AuthenticationService.AuthApi.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region constants messages
        private const string _DEFAULT_ERROR_MESSAGE = "Ocorreu um erro interno no servidor ";
        private const string _INVALID_MODEL_MESSAGE = "O objeto recebido tem campos obrigatórios nulos";
        private const string _INVALID_USER_OR_PASSWORD = "Usuário e/ou senha inválidos";
        private const string _INVALID_SESSION = "Sessão inválida";
        private const string _FORBIDDEN = "Não autorizado";
        private const bool _IS_UPDATE = true;
        private const bool _IS_NOT_UPDATE = false;
        #endregion

        #region attributes
        private readonly TokenGenerationService _tokenGenerator;
        private readonly UsuarioValidator _validator;
        private readonly IUsuarioService _usuarioService;
        private readonly KeyHasherService _keyHasherService;
        private readonly IConfiguration _config;

        #endregion 

        public AuthenticationController(
            IUsuarioService usuarioService, 
            UsuarioValidator validator, 
            TokenGenerationService tokenGenerator,
            KeyHasherService keyHasherService,
            IConfiguration config)
        {
            _usuarioService = usuarioService;
            _validator = validator;
            _tokenGenerator = tokenGenerator;
            _keyHasherService = keyHasherService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        protected JsonResult SignUp(
            [FromBody] UsuarioDto usuarioRequisicao,
            [FromServices]SigningConfigurations signingConfigurations)
        {
            
            try{
                Usuario usuario = UsuarioMapper(usuarioRequisicao);
                var results = _validator.Validate(usuario);

                if(results.IsValid){
                        //Atribui token
                        usuario.Token = GenerateToken(
                            usuario, 
                            signingConfigurations);

                        //Transforma a senha informada em um hash
                        usuario.Senha = _keyHasherService.EncriptKey(usuario.Senha);

                        //Guarda o token original e transforma o token da entidade em um hash para persistir
                        var token = usuario.Token;
                        usuario.Token = _keyHasherService.EncriptKey(usuario.Token);

                        //Atribui data de criação e ultimo logon (DateTime.Now)     
                        _usuarioService.PrepareEntityToSaveOrUpdate(usuario, _IS_NOT_UPDATE);

                        _usuarioService.Add(usuario);

                        //Limpa a senha para retornar a entidade ao cliente
                        usuario.Senha = String.Empty;

                        //Devolve o token original para retornar a entidade ao cliente
                        usuario.Token = token;

                        return new JsonResult(usuario);
                }else{
                    return new JsonResult(GetResponseErrorObj(
                        _INVALID_MODEL_MESSAGE, (int)HttpStatusCode.BadRequest));
                }
            }catch(NullUserException e){
                return new JsonResult(GetResponseErrorObj(
                    e.Message, (int)HttpStatusCode.BadRequest));
            }
            catch
            {
                return new JsonResult(GetResponseErrorObj(
                    _DEFAULT_ERROR_MESSAGE, (int)HttpStatusCode.InternalServerError));
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        protected JsonResult Login([FromBody] CredentialsDto credentials,
            [FromServices]SigningConfigurations signingConfigurations)
        {
            var passwordIsValid = false;
            Usuario user;
            try{
                user = _usuarioService.GetByEmail(credentials.email);
            }catch{
                return new JsonResult(GetResponseErrorObj(
                    _DEFAULT_ERROR_MESSAGE, (int)HttpStatusCode.InternalServerError));                    
            }

            if(user != null){
                passwordIsValid =_keyHasherService.VerifyKey(credentials.senha, user.Senha);
            }else{
                return new JsonResult(GetResponseErrorObj(
                    _INVALID_USER_OR_PASSWORD, (int)HttpStatusCode.Unauthorized));
            }

            if(passwordIsValid){

                //Atribui token
                user.Token = GenerateToken(
                    user, 
                    signingConfigurations);

                //Guarda o token original e transforma o token da entidade em um hash para persistir
                var clientToken = user.Token;
                user.Token = _keyHasherService.EncriptKey(user.Token);

                //Atribui apenas a data de atualização
                var preparedUser = _usuarioService
                    .PrepareEntityToSaveOrUpdate(user, _IS_UPDATE);

                try{
                    _usuarioService.Update(preparedUser);
                }catch{
                    return new JsonResult(GetResponseErrorObj(
                        _DEFAULT_ERROR_MESSAGE, (int)HttpStatusCode.InternalServerError));                    
                }

                user.Token = clientToken;
            }else{
                return new JsonResult(GetResponseErrorObj(
                    _INVALID_USER_OR_PASSWORD, (int)HttpStatusCode.Unauthorized));
            }

            return new JsonResult(user);
        }

        
        [HttpPost("{id}")]
        [Route("Profile")]
        protected JsonResult Profile([FromQuery]string id, [FromHeader] string Bearer)
        {
            Usuario usuario;
            bool tokenIsValid;

            if(String.IsNullOrEmpty(Bearer)){
                return new JsonResult(GetResponseErrorObj(
                    _FORBIDDEN, (int)HttpStatusCode.Unauthorized));
            }            

            if(String.IsNullOrEmpty(id)){
                return new JsonResult(GetResponseErrorObj(
                    _FORBIDDEN, (int)HttpStatusCode.Unauthorized));
            } 

            try{
                var internalId = Guid.Parse(id);
                usuario = _usuarioService.Select(internalId);
            }catch{
                return new JsonResult(GetResponseErrorObj(
                    _FORBIDDEN, (int)HttpStatusCode.InternalServerError));      
            }

            if(usuario != null){
                tokenIsValid =_keyHasherService.VerifyKey(Bearer, usuario.Token);
            }else{
                return new JsonResult(GetResponseErrorObj(
                    _FORBIDDEN, (int)HttpStatusCode.Unauthorized));
            }

            if(tokenIsValid){

                var elapsedTimeSinceLastLogon = DateTime.Now.Minute - usuario.UltimoLogin.Minute;

                if(elapsedTimeSinceLastLogon >= 30){
                    return new JsonResult(GetResponseErrorObj(
                   _INVALID_SESSION, (int)HttpStatusCode.Unauthorized));
                }

            }else{
                return new JsonResult(GetResponseErrorObj(
                    _FORBIDDEN, (int)HttpStatusCode.Unauthorized));
            }

            usuario.Senha = String.Empty;
            usuario.Token = Bearer;

            return new JsonResult(usuario);
        }

        private Usuario UsuarioMapper(UsuarioDto usuarioRequisicao)
        {
            //==> SUBSTITUIR PELO USO DE AUTOMAPER===
            if(usuarioRequisicao != null){
                var usuario = new Usuario
                {
                    Nome = usuarioRequisicao.nome,
                    Email = usuarioRequisicao.email,
                    Senha = usuarioRequisicao.senha
                };

                if (usuarioRequisicao.telefones != null){
                    usuario.Telefones = new List<Telefone>();
                    usuario.Telefones.AddRange(usuarioRequisicao.telefones);
                }
                return usuario;            
            }else{
                throw new NullUserException("Dados inválidos");
            }         
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
            SigningConfigurations signingConfigurations)
        {            
            return _tokenGenerator.GenerateToken(usuario, signingConfigurations, _config);
        }
        
    }
}
