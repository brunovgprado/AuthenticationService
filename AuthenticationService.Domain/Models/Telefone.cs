using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Models
{
    public class Telefone
    {
        public int Id { get; set; }
        public Guid UsuarioFK { get; set; }
        public virtual Usuario User { get; set; }
        public string Numero { get; set; }
        public string DDD { get; set; }
    }
}
