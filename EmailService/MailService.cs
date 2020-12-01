using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailService
{
    public class MailService: IMailService
    {
        private readonly MailSettings _settings;

        public MailService(MailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendAsync(string to, string displayName, string subject, string html)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress fromEmail = new MailboxAddress(Encoding.Default, _settings.DisplayName, _settings.Mail);
            message.From.Add(fromEmail);

            MailboxAddress toEmail = new MailboxAddress(Encoding.Default, displayName, to);
            message.To.Add(toEmail);

            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = html;

            message.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_settings.Mail, _settings.Password);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
