using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PruebaProgra2.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        // Constructor donde se inyecta la configuración de EmailSettings
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer))
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.EmailFrom),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true, // Si el cuerpo del correo es en HTML
                };
                mailMessage.To.Add(toEmail);

                // Configura las credenciales y puerto
                smtpClient.Port = _emailSettings.SmtpPort;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.EmailFrom, _emailSettings.EmailPassword);
                smtpClient.EnableSsl = true; // Habilita la seguridad SSL

                // Envía el correo electrónico
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }

    // Clase para almacenar la configuración del correo
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
    }
}
