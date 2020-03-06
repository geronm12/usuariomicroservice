using MicroServicioUsuarios.Whatsapp.ExtensionesYHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp
{
    public interface IWhatsappSender
    {
        Task<MensajeResult> EnviarMensaje(WhatsappSenderDto modeloMensaje, FormaVerificacion método, string email);

        Task<MensajeResult> VerificarCódigo(WhastappUserVerificationDto modeloMensaje); 

    }
}