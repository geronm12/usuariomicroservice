using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string PasswordCode { get; set; }

    }
}
