using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using System;

namespace AuthenticationService.Domain.Services
{

    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        public UsuarioService(IUsuarioRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public Usuario GetByEmail(string email)
        {
            return _repository.GetUsuarioByEmail(email);
        }

        public Usuario PrepareEntityToSaveOrUpdate(Usuario usuario, bool isUpdate){            
            
            usuario.UltimoLogin = DateTime.Now;

            if(!isUpdate)
                usuario.DataCriacao = DateTime.Now;

            return usuario;
        }
    }
}
