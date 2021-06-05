using System.Threading.Tasks;

namespace MinhaLoja.Core.Infra.Services.Mail
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body);
    }
}
