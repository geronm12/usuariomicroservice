

namespace Mailer
{
    using System.Collections.Generic;
    /// <summary>
    /// Response de la clase SendGrid
    /// </summary>
    public class SendGridResponse
   {
     ///<summary>
     ///Errores del response
     /// </summary>
     
    public List<SendGridErrorResponse> Errores { get; set; }

   }
    
}
