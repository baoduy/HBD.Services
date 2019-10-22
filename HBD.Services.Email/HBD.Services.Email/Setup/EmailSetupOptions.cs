using System;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using HBD.Services.Email.Builders;

namespace Microsoft.Extensions.DependencyInjection
{
    public class EmailSetupOptions
    {
        internal string FromEmail { get; private set; }
        internal Func<SmtpClient> SmtpClientFactory { get; private set; }
        internal bool SmtpClientFromConfig { get; private set; }
        internal string JsonFile { get; private set; }
        internal bool SectionName { get; private set; }
        internal Action<IEmailTemplateBuilder> TemplateBuilder { get; private set; }

        public EmailSetupOptions From(string email)
        {
            Contract.Requires(string.IsNullOrEmpty(email) == false);

            FromEmail = email;
            return this;
        }

        public EmailSetupOptions WithSmtp(Func<SmtpClient> smtpClientFactory)
        {
            Contract.Requires(smtpClientFactory != null);

            SmtpClientFromConfig = false;
            SmtpClientFactory = smtpClientFactory;
            return this;
        }

        public EmailSetupOptions WithSmtpFromConfiguration()
        {
            SmtpClientFactory = null;
            SmtpClientFromConfig = true;
            return this;
        }

        public EmailSetupOptions EmailTemplateFromFile(string jsonFile)
        {
            Contract.Requires(!string.IsNullOrEmpty(jsonFile));

            JsonFile = jsonFile;
            return this;
        }

        /// <summary>
        /// Load Email Templates from appSetting.json section.
        /// </summary>
        /// <returns></returns>
        public EmailSetupOptions EmailTemplateFromConfiguration()
        {
            SectionName = true;
            if (SmtpClientFactory == null)
                SmtpClientFromConfig = true;
            return this;
        }

        public EmailSetupOptions EmailTemplateFrom(Action<IEmailTemplateBuilder> builder)
        {
            Contract.Requires(builder != null);

            TemplateBuilder = builder;
            return this;
        }
    }
}
