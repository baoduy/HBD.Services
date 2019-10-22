using HBD.Services.Email.Exceptions;
using HBD.Services.Email.Templates;
using HBD.Services.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HBD.Services.Email.Providers
{
    public class MailMessageProvider : IMailMessageProvider
    {
        #region Fields

        private readonly IList<IEmailTemplateProvider> _emailTemplateProvider;
        private readonly ITransformer _transformer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Use custom ITransformer
        /// </summary>
        /// <param name="emailTemplateProvider"></param>
        /// <param name="transformer"></param>
        public MailMessageProvider(IEnumerable<IEmailTemplateProvider> emailTemplateProvider, ITransformer transformer)
        {
            _emailTemplateProvider = emailTemplateProvider.ToList();
            this._transformer = transformer ?? throw new ArgumentNullException(nameof(transformer));
        }

        /// <summary>
        /// Use custom ITransformer
        /// </summary>
        /// <param name="emailTemplateProvider"></param>
        /// <param name="transformer"></param>
        protected internal MailMessageProvider(IEmailTemplateProvider emailTemplateProvider, ITransformer transformer)
            : this(new[] { emailTemplateProvider }, transformer)
        {
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
        }

        public virtual async Task<MailMessage> GetMailMessageAsync(string templateName, object[] transformData, params string[] attachments)
        {
            var template = await GetTemplate(templateName).ConfigureAwait(false);
            if (template == null)
                throw new TemplateNotFoundException(templateName);

            return await GetMailMessageAsync(template, transformData, attachments).ConfigureAwait(false);
        }

        protected async Task<MailMessage> GetMailMessageAsync(IEmailTemplate template, object[] transformData, params string[] attachments)
        {
            var mail = new MailMessage();

            //Remove the AngledBracketTokenExtractor from Transformer as it might impacts to the html format.

            await Task.WhenAll(
                mail.To.FromAsync(template.ToEmails, _transformer, transformData),
                mail.CC.FromAsync(template.CcEmails, _transformer, transformData),
                mail.Bcc.FromAsync(template.BccEmails, _transformer, transformData)
            ).ConfigureAwait(false);

            mail.Subject = await _transformer.TransformAsync(template.Subject, transformData).ConfigureAwait(false);
            mail.Body = await _transformer.TransformAsync(template.Body, transformData).ConfigureAwait(false);
            mail.IsBodyHtml = template.IsBodyHtml;

            return mail;
        }

        private async Task<IEmailTemplate> GetTemplate(string templateName)
        {
            foreach (var p in _emailTemplateProvider)
            {
                var t = await p.GetTemplate(templateName).ConfigureAwait(false);
                if (t != null) return t;
            }
            return null;
        }

        #endregion Methods
    }
}