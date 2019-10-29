using AuthenticationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Interfaces.Services
{
    public interface IUsuarioApplicationService
    {
        Usuario Add(Usuario addUser);
        Usuario Select(Guid userId);
    }
}
