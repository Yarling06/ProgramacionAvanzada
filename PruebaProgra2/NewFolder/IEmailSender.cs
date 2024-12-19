using System.Threading.Tasks;

namespace PruebaProgra2.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
