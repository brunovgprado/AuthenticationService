using AuthenticationService.Data.Context;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Data.Repository
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AuthenticationContext dbContext) : base(dbContext)
        {
        }

        public Usuario GetUsuarioByEmail(string email)
        {
           return dbContext.Usuarios.Where(u => u.Email == email).FirstOrDefault();
        }
    }
}
