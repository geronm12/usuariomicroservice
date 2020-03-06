using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.CineEmailSender
{
    public class ConfigExtension
    {
        public static IConfiguration _config;

        public ConfigExtension(IConfiguration config)
        {
            _config = config;
        }
    }



    public static class CineAunthenticationMailSender
    { 
        public static string BodyContentVerificationMail(string displayName, string verificationURL)
        {
             return "Mail de Verificación" +
                    $"Hola!! {displayName}, " +
                    "Gracias por crearte una cuenta con nosotros. Para continuar, por favor, verifica tu Email haciendo" +
                    "click en el siguiente link:" + " " +
                     verificationURL;
        }

        public static string BodyContentResetPasswordEmail(string displayName, string resetPasswordUrl)
        {
            return "Reseteo de Password" +
                    $"Hola! {displayName}, " +
                    "Sentimos que hayas olvidado tu contraseña :( ingresa al siguiente link para poder cambiarla:" + " " +
                     resetPasswordUrl;
        }


        public static async Task<SendEmailResponse> SendUserVerificationEmail(string displayName, string email, string verificationURL, 
            string body)
        {
            IEmailSender _sender = new SendGridEmailSender(ConfigExtension._config);

            var response =  await Task.Run(() =>
            {
                return  _sender.SendEmailAsync(new SendEmailDetails
                {
                    IsHtml = true,
                    FromEmail = ConfigExtension._config["SendGrid:FromEmail"],
                    FromName = ConfigExtension._config["SendGrid:FromName"],
                    ToEmail = email,
                    ToName = displayName,
                    Subject = "Mail de Verificación",
                    BodyContent = body
                });

            });
           

            return  response;

          
        }
    }

    

      


       
    
}
