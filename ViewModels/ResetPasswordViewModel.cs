using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }

        public string Mail { get; set; }

    }
}
