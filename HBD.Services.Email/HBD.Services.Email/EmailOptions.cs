using System;
using System.Collections.Generic;
using System.Net.Mail;
using HBD.Services.Email.Configurations;

namespace HBD.Services.Email
{
    public class EmailOptions
    {
        public MailAddress FromEmailAddress { get; set; }

        public IList<EmailTemplate> Templates { get; set; } = new List<EmailTemplate>();

        /// <summary>
        /// Load Email Templates from Json file.
        /// </summary>
        public string TemplateJsonFile { get; set; }

        public Func<SmtpClient> SmtpClientFactory { get; set; } = () => new SmtpClient();
    }
}
