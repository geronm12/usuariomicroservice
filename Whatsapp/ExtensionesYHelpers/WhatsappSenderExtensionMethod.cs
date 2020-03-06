using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp
{
    public static class WhatsappSenderExtensionMethod
    { 
        public static string GenerarCodigoVerificacion(this WhatsappSenderDto sender)
        {
            var generador = new Random();

            int[] codigo = new int[4];

            for (int i = 0; i < codigo.Length; i++)
            {
                codigo[i] = generador.Next(0, 9);
            }

            return String.Join("", codigo.Select(p => p.ToString()).ToArray());

        }



    }
}
