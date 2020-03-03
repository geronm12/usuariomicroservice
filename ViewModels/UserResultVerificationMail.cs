using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ViewModels
{
    public class UserResultVerificationMail
    {
        public string UID { get; set; }

        public string EmailCode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

    }
}
