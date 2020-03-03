using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Servicios.UserService
{
    public class LoginDto
    {
        public string UsuarioOEmail { get; set; }

        public string Password { get; set; }

    }
}
