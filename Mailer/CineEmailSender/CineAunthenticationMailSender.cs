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
         
        public static async Task<SendEmailResponse> SendUserVerificationEmail(string displayName, string email, string verificationURL)
        {
           IEmailSender _sender = new SendGridEmailSender(ConfigExtension._config);

           
           return await _sender.SendEmailAsync(new SendEmailDetails
           {
               IsHtml = true,
               FromEmail = ConfigExtension._config["SendGrid:FromEmail"],
               FromName = ConfigExtension._config["SendGrid:FromName"],
               ToEmail = email,
               ToName = displayName,
               Subject = "Mail de Verificación",
               BodyContent = "Verify Email"+ 
               $"Hi {displayName},"+
               "Thanks for creating an account with us. To continue please verify your email with us."+
               "Verify Email"+
               verificationURL

           });

        }

    }
}
