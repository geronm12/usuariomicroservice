namespace Mailer
{
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class SendGridEmailSender:IEmailSender
    {
            private static IConfiguration _config { get; set; }
            
         
        public SendGridEmailSender(IConfiguration config)
        {
           
           _config = config;
        
        }
 

        public async Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details)
            {

            ///<summary>
            ///Api generada en SendGrid 
            /// </summary>
            var apiKey = _config["SendGrid:ApiKey"];
               ///<summary>
               ///Clase SendGridClient que requiere como parámetros apiKey
               ///Posee 4 sobrecargas para más información navegar dentro de la clase.
               /// </summary>    
             var client = new SendGridClient(apiKey);
               //From
               var from = new EmailAddress(details.FromEmail, details.FromName);
               //Subject   
               var subject = details.Subject;
               //To    
               var to = new EmailAddress(details.ToEmail, details.ToName);
               //Content    
               var plainTextContent = details.BodyContent;
               //Crea la clase email lista para enviar    
                var msg = MailHelper.CreateSingleEmail(from, 
                    to,
                    subject, 
                    //Texto plano
                    details.IsHtml ? null : details.BodyContent,
                    //Html content
                    details.IsHtml ? details.BodyContent : null);
 

            var response = await client.SendEmailAsync(msg);
            
            ///<summary>
            ///Si el mail es aceptado exitosamente
            /// </summary>

            if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return new SendEmailResponse();
            }
            
            ///<summary>
            ///De lo contrario se ejecutará la respuesta al error
            /// </summary>
            try
            {
                var bodyResult = await response.Body.ReadAsStringAsync();

                //Deserializar la respuesta

                var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);

                //Agregar errores a la lista 
                var errorResponse = new SendEmailResponse
                {
                    Errors = sendGridResponse?.Errores.Select(x => x.Message).ToList()

                };

                //Asegurarnos de tener aunque sea un error

                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    //Agregar un error desconocido
                    errorResponse.Errors = new List<string>(new[] { "Ocurrió un error desconocido. Por favor contacte con soporte" });                

            }
            catch(Exception ex)
            {
                //


                //Rompe si estamos debuggeando

                if (Debugger.IsAttached)
                {
                    var error = ex;
                    Debugger.Break();
                }
                  

                //Si ocurre algo inesperado devuelve el mensaje
                return new SendEmailResponse
                {
                    Errors = new List<string>(new[] {"Ocurrió un error inesperado" })
                };


            }


            return new SendEmailResponse();
        }
        }
    }


