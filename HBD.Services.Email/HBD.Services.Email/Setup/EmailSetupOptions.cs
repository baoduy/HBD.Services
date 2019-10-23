using HBD.Services.Email.Builders;
using HBD.Services.Transformation;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.Contracts;
using System.Net.Mail;

namespace Microsoft.Extensions.DependencyInjection
{
    public class EmailSetupOptions
    {
        #region Properties

        internal string FromEmail { get; private set; }

        internal string JsonFile { get; private set; }

        internal IConfigurationSection ConfigSection { get; private set; }

        internal Func<SmtpClient> SmtpClientFactory { get; private set; }

        internal Action<IEmailTemplateBuilder> TemplateBuilder { get; private set; }

        internal Action<TransformOptions> TransformOptions { get; private set; }
        #endregion Properties

        #region Methods

        public EmailSetupOptions EmailTemplateFrom(Action<IEmailTemplateBuilder> builder)
        {
            Contract.Requires(builder != null);

            TemplateBuilder = builder;
            return this;
        }

        public EmailSetupOptions EmailTemplateFromFile(string jsonFile)
        {
            Contract.Requires(!string.IsNullOrEmpty(jsonFile));

            JsonFile = jsonFile;
            return this;
        }

        public EmailSetupOptions FromEmailAddress(string email)
        {
            Contract.Requires(string.IsNullOrEmpty(email) == false);

            FromEmail = email;
            return this;
        }

        public EmailSetupOptions WithSmtp(Func<SmtpClient> smtpClientFactory)
        {
            Contract.Requires(smtpClientFactory != null);

            SmtpClientFactory = smtpClientFactory;
            return this;
        }

        /// <summary>
        /// The Configuration need to be provided both SmtpClient and Templates info.
        /// </summary>
        /// <returns></returns>
        public EmailSetupOptions FromConfiguration(IConfigurationSection section)
        {
            SmtpClientFactory = null;
            ConfigSection = section;
            return this;
        }

        public EmailSetupOptions WithTransformer(Action<TransformOptions> transformOptions)
        {
            this.TransformOptions = transformOptions;
            return this;
        }
        #endregion Methods
    }
}