using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Servicios.UserService
{
    public class Result
    {
        public bool Response { get; set; }

        public string ErrorMsg { get; set; }

        public string Body { get; set; }

    }
}
