using MicroServicioUsuarios.Whatsapp.ExtensionesYHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp
{

    /// <summary>
    /// Clase que implementará los parámetros necesarios para enviar el código de verificación
    /// </summary>
    public class WhatsappSenderDto
    {
      
        //Código telefónico del país EJ: ARG +54 9
        public string CodPais { get; set; }

        //Código de área de la ciudad (En caso de ser Tucumán por ej: 381)
        public string CodCiudad { get; set; }

        //Número del celular ej: 555 333
        public string NumeroCelular { get; set; }

        //Campo calculado que junta todas las propiedades anteriores en un string
        public string Telefono => CodPais + CodCiudad + NumeroCelular;

        //Método de verificaicón  puede ser vía whatsapp(por defecto), sms (si se compra el n° twilio y se lo configura), llamada (se configura el servicio enn twilio)..
        public FormaVerificacion Verificacion { get; set; }

        //Mensaje por defeecto que se enviará a su teléfono con el código de verificación
        public string Mensaje => "Su código de verficación es:";


 
     }
}
