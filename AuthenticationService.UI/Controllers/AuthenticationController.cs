using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using AuthenticationService.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.UI.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthenticationController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [Route("SigUp")]
        public JsonResult SignUp([FromBody] Usuario usuario)
        {
            return new JsonResult(usuario);
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
    }
}
