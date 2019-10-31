using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AuthenticationService.Domain.Models;
using AuthenticationService.UI.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.UI.Services
{
    public class TokenGeneratioService
    {
        public string GenerateToken(
            Usuario usuario, 
            SigningConfigurations signingConfigurations, 
            TokenConfigurations tokenConfigurations
        )
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
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            
            return handler.WriteToken(securityToken);
        }
    }
}