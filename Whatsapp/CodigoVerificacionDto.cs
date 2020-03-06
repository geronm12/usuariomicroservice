using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp
{
    public class CodigoVerificacionDto
    {
        public Guid Id { get; set; }

        public string Código { get; set; }

        public string UserId { get; set; }
    }
}
