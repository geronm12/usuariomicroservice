using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp
{
    public class TwilioResponse
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

        public string Codigo { get; set; }
    }
}
