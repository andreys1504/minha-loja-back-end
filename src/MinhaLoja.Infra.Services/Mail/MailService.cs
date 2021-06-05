using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MinhaLoja.Core.Infra.Services.Mail;
using MinhaLoja.Core.Settings;
using System.Threading.Tasks;

namespace MinhaLoja.Infra.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly GlobalSettings _globalSettings;

        public MailService(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public async Task SendMailAsync(
            string to,
            string subject,
            string body)
        {
            if (_globalSettings.TriggerEmails is false)
                return;

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_globalSettings.SmtpClient.EmailSupport));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(
                _globalSettings.SmtpClient.Server,
                _globalSettings.SmtpClient.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _globalSettings.SmtpClient.Username,
                _globalSettings.SmtpClient.Password);

            await smtp.SendAsync(mimeMessage);
            await smtp.DisconnectAsync(true);
        }
    }
}
