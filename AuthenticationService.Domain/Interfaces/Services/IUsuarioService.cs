using AuthenticationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Interfaces.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario PrepareEntityToSave(Usuario usuario);
    }
}
