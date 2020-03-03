
namespace Mailer
{
    using System.Threading.Tasks;

    /// <summary>
    /// Envia el mail usando the <see cref="IEmailSender"/> y creando el template
    /// específico desde el usuario remitente
    /// </summary>
    public interface IEmailTemplateSender
    {
        /// <summary>
        /// Envia un email usando los detalles otorgados por parámetro. NOTA: La propiedad CONTENT es ignorada y reemplazada por el template.
        /// </summary>
        /// <param name="details">Objeto de tipo <see cref="SendEmailDetails"/> 
        /// </param>
        /// <param name="title">Titulo que ira en el mail</param>
        /// <param name="content">Contenido del primer párrafo del mail</param>
        /// <param name="content2">Contenido del segundo párrafo del mail</param>
        /// <param name="buttonText">Texto del botón</param>
        /// <param name="buttonUrl">Dirección a la que nos redigirá el botón</param>
        /// <returns></returns>

        Task<SendEmailResponse>SendGeneratedEmailAsync(SendEmailDetails details, string title, string content, string content2, string buttonText, string buttonUrl);

    


    }
}
