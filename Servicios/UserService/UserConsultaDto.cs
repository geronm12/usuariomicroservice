using MicroServicioUsuarios.Whatsapp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Servicios.UserService
{
    public class UserConsultaDto
    {
        public UserDto Usuario { get; set; }

        public string Codigo { get; set; }

    }
}
