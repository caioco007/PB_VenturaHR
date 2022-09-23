using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Services.Mail
{
    public class MailService
    {
        IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendAsync(string title, string htmlContent, List<string> mailAddresses, List<Attachment> attachments, bool useContact = false)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient()
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    UseDefaultCredentials = false,
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("caiocogomes@gmail.com", "0906Cfsc*")
                };

                MailMessage mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    From = new MailAddress("caiocogomes@hotmail.com", "VenturaHR")
                };

                mailMessage.Subject = title;
                mailMessage.To.Add("caio.gomes@al.infnet.edu.br");
                string _htmlContent =
                        "<html>" +
                        "<head>" +
                        "</head>" +
                        "<body>" +
                        "<div style='text-align:center;'>" +
                        "<br/><br/>" +
                        "</div>" +
                        "<div>" +
                        htmlContent +
                        "</div>" +
                        "</body>" +
                        "</html>";

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(_htmlContent, null, MediaTypeNames.Text.Html);
                if (attachments != null)
                    foreach (var attachment in attachments)
                        mailMessage.Attachments.Add(attachment);
                mailMessage.AlternateViews.Add(htmlView);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception e)
            {

            }
        }

    }
}
