using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.Templates
{
    /// <summary>
    /// Implementa la interfaz <see cref="IEmailTemplateSender"/> 
    /// </summary>
    public class EmailTemplateSender : IEmailTemplateSender
    {
        private readonly IEmailSender _sender;
        public EmailTemplateSender(IEmailSender sender)
        {
            _sender = sender;
        }

        public async Task<SendEmailResponse>SendGeneratedEmailAsync(SendEmailDetails details, string title, string content, string content2, string buttonText, string buttonUrl)
        {

            var templateText = default(string);

            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Mailer.Templates.MailerTemplate.html"), Encoding.UTF8))
            {
                
                templateText = await reader.ReadToEndAsync();

            }


            templateText = templateText.Replace("--Title--", title).Replace("--Content1--", content).Replace("--Content2--", content2)
                .Replace("--ButtonText--", buttonText).Replace("--ButtonUrl--", buttonUrl);

            details.BodyContent = templateText;

            return await _sender.SendEmailAsync(details);
        }
    }
}
