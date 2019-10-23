using System.Collections.Generic;

namespace HBD.Services.Email.Templates
{
    public class EmailTemplateSection
    {
        #region Properties

        public bool EnableSsl { get; set; }

        public string FromEmail { get; set; }

        public string Host { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public IList<EmailTemplate> Templates { get; } = new List<EmailTemplate>();

        public string UserName { get; set; }

        #endregion Properties
    }
}