
namespace Mailer
{
    /// <summary>
    /// <parms>Detalles sobre el email a enviar</parms>
    /// </summary>
    public class SendEmailDetails
    {
        /// <summary>
        /// Nombre del remitente
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Mail del remitente
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Nombre del destinatario
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Email del destinatario
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// Asunto del Mail
        /// </summary>
        public string Subject { get; set; }


        /// <summary>
        /// Cuerpo del Mail
        /// </summary>
        public string BodyContent { get; set; }


        /// <summary>
        /// Indica si el email es HTML
        /// </summary>
        public bool IsHtml { get; set; }
    }
}
