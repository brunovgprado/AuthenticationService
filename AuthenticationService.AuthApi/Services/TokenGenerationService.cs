using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AuthenticationService.Domain.Models;
using AuthenticationService.AuthApi.ConfigurationsApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.AuthApi.Services
{
    public class TokenGenerationService
    {
        public string GenerateToken(
            Usuario usuario, 
            SigningConfigurations signingConfigurations,
            IConfiguration config)
        {
                
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(usuario.Email),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString())
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(1800);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = config["TokenConfigurations:Issuer"],
                Audience = config["TokenConfigurations:Audience"],
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            
            return handler.WriteToken(securityToken);
        }
    }
}