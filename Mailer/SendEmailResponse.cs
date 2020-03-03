

namespace Mailer
{
    using System.Collections.Generic;
    /// <summary>
    /// Respuesta del paquete nugget SendEmail llamada por <see cref="IEmailSender"/> implementation
    /// </summary>
    public class SendEmailResponse
    {
        /// <summary>
        /// Indica si fue exitoso el envío del email
        /// </summary>
        public bool Succesfull => !(Errors?.Count > 0);

        ///<summary>
        /// Propiedad que indica el mensaje de error en caso de que hubiera
        /// </summary>

        public List<string> Errors { get; set; }


    }
}
