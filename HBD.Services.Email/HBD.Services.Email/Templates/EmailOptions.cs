using System;
using System.Net.Mail;

namespace HBD.Services.Email.Templates
{
    public class EmailOptions
    {
        public MailAddress FromEmailAddress { get; set; }

        public Func<SmtpClient> SmtpClientFactory { get; set; } = () => new SmtpClient();
    }
}
