using System;
using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Services;
using AuthenticationService.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.UI.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string DEFAULT_ERROR_MESSAGE = "Ocorreu um erro interno no servidor ";
        private readonly UsuarioValidator _validator;
        private readonly IUsuarioService _usuarioService;

        public AuthenticationController(IUsuarioService usuarioService, UsuarioValidator validator)
        {
            _usuarioService = usuarioService;
            _validator = validator;
        }

        [HttpPost]
        [Route("SigUp")]
        public JsonResult SignUp([FromBody] Usuario usuario)
        {
            var results = _validator.Validate(usuario);

            if(results.IsValid){
                try{
                    _usuarioService.Add(usuario);
                    return new JsonResult(usuario);
                }catch(Exception e)
                {
                    return new JsonResult(
                        new ResponseMesageViewModel()
                        { statusCode = 500, 
                        mensagem = string.Format(DEFAULT_ERROR_MESSAGE, 
                        e.Message.ToString())});
                }
            }else{
                return new JsonResult(
                        new ResponseMesageViewModel()
                        { statusCode = 500, 
                        mensagem = string.Format(DEFAULT_ERROR_MESSAGE)});
            }
        }

        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromBody] CredentialsModel credentials)
        {
            return new JsonResult(credentials);
        }

        [HttpPost("{id}")]
        [Route("Profile")]
        public void Profile([FromQuery]string id, [FromHeader] string Bearer)
        {

        }

        [HttpGet("{id}")]
        [Route("Test")]
        public JsonResult TestGetUser([FromQuery]string id)
        {
            var internalId = Guid.Parse(id);
            var usuario = _usuarioService.Select(internalId);
            return new JsonResult(usuario);
        }
    }
}
