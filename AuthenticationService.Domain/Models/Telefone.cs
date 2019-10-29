using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Models
{
    public class Telefone
    {
        public Guid Id { get; set; }
        public virtual Usuario user { get; set; }
        public string Numero { get; set; }
        public string DDD { get; set; }
    }
}
