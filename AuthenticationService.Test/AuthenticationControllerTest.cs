using AuthenticationService.AuthApi.Controllers;
using AuthenticationService.Domain.Services;
using AuthenticationService.Domain.Models;

namespace AuthenticationService.Test
{
    public class AuthenticationControllerTest
    {
        AuthenticationController _controller;        
        
        public AuthenticationControllerTest(){
            _controller = new AuthenticationController();
        }

        public void Login_WhenCalledWithNullObject_ReturnsBadRequestResult(){
            
        }
    }
}