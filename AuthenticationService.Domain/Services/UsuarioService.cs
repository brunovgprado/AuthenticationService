using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using System;

namespace AuthenticationService.Domain.Services
{

    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        public UsuarioService(IRepositoryBase<Usuario> repository) : base(repository)
        {
        }
        public Usuario GetByEmail(string email)
        {
            return GetByEmail(email);
        }
    }
}
