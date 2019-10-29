using AuthenticationService.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Services
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {           
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome não informado");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email não informado");
            RuleFor(x => x.Senha).NotEmpty().WithMessage("Senha não informada");
        }
    }
}
