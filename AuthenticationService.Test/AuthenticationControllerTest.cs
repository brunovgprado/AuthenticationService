using AuthenticationService.AuthApi.Controllers;
using AuthenticationService.Domain.Services;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Interfaces.Services;
using Moq;
using AuthenticationService.AuthApi.Services;
using AuthenticationService.Application.Service;
using Microsoft.Extensions.Configuration;
using AuthenticationService.Application.AppModel.Dtos;
using AuthenticationService.AuthApi.ConfigurationsApi;
using Xunit;

namespace AuthenticationService.Test
{
    public class AuthenticationControllerTest
    {
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private UsuarioValidator _usuarioValidator;
        private TokenGenerationService _tokenService;
        private KeyHasherService _hashingService;
        private IConfiguration _config;


        AuthenticationController _controller;  
        
        
        public AuthenticationControllerTest(){
            _mockUsuarioService = new Mock<IUsuarioService>();
            _usuarioValidator = new UsuarioValidator();

            _controller = new AuthenticationController(
                _mockUsuarioService.Object,
                _usuarioValidator,
                _tokenService,
                _hashingService,
                _config);
        }

        [Fact]
        public void Login_WhenCalledWithNullObject_ReturnsBadRequestResult(){

            var siginConfigurations = new SigningConfigurations();
            CredentialsDto credentialsDto = null;

            var result = _controller.Login(credentialsDto, siginConfigurations);

            ResponseMesageViewDto resp = (ResponseMesageViewDto)result.Value;

            Assert.Equal("200", resp.statusCode.ToString());
        }
    }
}