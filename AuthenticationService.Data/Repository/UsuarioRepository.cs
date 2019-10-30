using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Data.Repository
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
    }
}
