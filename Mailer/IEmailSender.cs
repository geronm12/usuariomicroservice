
namespace Mailer
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interfaz correspondiente a <see cref="SendGridEmailSender"/>
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// </summary>
        /// <param name="details">Detalles sobre el email a enviar</param>
        /// <returns></returns>
        Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details);

    }
}
