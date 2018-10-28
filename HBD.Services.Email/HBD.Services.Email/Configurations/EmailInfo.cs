using System;
using System.IO;
using HBD.Services.Email.Exceptions;

namespace HBD.Services.Email.Configurations
{
    /// <summary>
    /// This 
    /// </summary>
    public class EmailInfo : IEmailInfo
    {
        private readonly EmailTemplate _template;

        public EmailInfo(EmailTemplate template)
        {
            this._template = template??throw new ArgumentNullException(nameof(template));

            if (!template.IsValid)
                throw new InvalidTemplateException(template);

            if (!string.IsNullOrWhiteSpace(template.ToEmails))
                this.ToEmails = template.ToEmails.SplitBySeparator();

            if (!string.IsNullOrWhiteSpace(template.CcEmails))
                this.CcEmails = template.ToEmails.SplitBySeparator();

            if (!string.IsNullOrWhiteSpace(template.BccEmails))
                this.BccEmails = template.ToEmails.SplitBySeparator();

            this.IsBodyHtml = template.IsBodyHtml;

            this.Subject = template.Subject;
            this.Body =!string.IsNullOrWhiteSpace( template.BodyFile) ? File.ReadAllText(template.GetBodyFile()) : template.Body;
        }

        public string Name => _template.Name;
        public string[] ToEmails { get; }
        public string[] CcEmails { get;  }
        public string[] BccEmails { get; }
        public string Subject { get; }
        public string Body { get; }
        public bool IsBodyHtml { get; }
    }
}
