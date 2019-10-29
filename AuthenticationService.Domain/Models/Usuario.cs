using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public virtual List<Telefone> Telefones { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime UltimoLogin { get; set; }
        public string Token { get; set; }
    }
}
