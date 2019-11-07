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
using System;

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

        public AuthenticationControllerTest() {
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
        public void Login_WhenCalledWithNull_ReturnsBadRequestResult()
        {

            var siginConfigurations = new SigningConfigurations();

            var result = _controller.Login(null, siginConfigurations);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void SignUp_WhenCalledWithObjectWithoutMandatoryInfo_ReturnsBadRequestResult()
        {
            var siginConfigurations = new SigningConfigurations();            

            var result = _controller.SignUp(
                new UsuarioDto() 
                {
                    nome = "Jon Doe",
                    email = "jondoe@dmain.com",
                    senha = ""
                }, 
                siginConfigurations);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Profile_WhenCalledWithInvalidToken_ReturnsUnautorizedResult()
        {

            var result = _controller.Profile(Guid.NewGuid().ToString(), "83xuryr38rmdx8743yrx12yr214");

            Assert.Equal(401, result.StatusCode);
        }
    }
}