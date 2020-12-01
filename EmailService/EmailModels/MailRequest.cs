using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EmailService.EmailModels
{
    public class MailRequest
    {
        public string toEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
