﻿using System.Collections.Generic;

namespace HBD.Services.Email.Templates
{
    public class EmailTemplateSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string FromEmail { get; set; }

        public IList<EmailTemplate> Templates { get; } = new List<EmailTemplate>();
    }
}
