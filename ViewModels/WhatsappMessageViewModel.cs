using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ViewModels
{
    public class WhatsappMessageViewModel
    {
        public string CodigoDePais { get; set; }

        public string CodigoDeCiudad { get; set; }

        public string NumCel { get; set; }

        public string Mensaje { get; set; }

        public string Telefono => CodigoDePais + CodigoDeCiudad + NumCel;

        public string UserName { get; set; }

    }
}
