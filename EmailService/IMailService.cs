using System.Threading.Tasks;

namespace EmailService
{
    public interface IMailService
    {
        public Task SendAsync(string to, string displayName, string subject, string html);
    }
}
