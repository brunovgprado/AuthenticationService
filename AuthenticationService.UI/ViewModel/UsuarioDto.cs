using System;
using System.Collections.Generic;
using AuthenticationService.Domain.Models;

namespace AuthenticationService.UI.ViewModel
{
    public class UsuarioDto
    {
        public Guid id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public virtual List<Telefone> telefones { get; set; }
        public DateTime dataCriacao { get; set; }
        public DateTime? dataAtualizacao { get; set; }
        public DateTime ultimoLogin { get; set; }
        public string token { get; set; }
    }
}