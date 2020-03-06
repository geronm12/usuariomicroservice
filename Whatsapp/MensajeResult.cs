using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp
{
    public class MensajeResult
    {
        public bool Succesful { get; set; }

        public string MsgError { get; set; }

        public string Body { get; set; }

    }
}
