using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.AppModel.Dtos
{
    public class CredentialsDto
    {
        public string email { get; set; }
        public string senha { get; set; }
    }
}
