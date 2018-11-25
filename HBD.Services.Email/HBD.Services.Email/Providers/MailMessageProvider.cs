using HBD.Services.Email.Exceptions;
using HBD.Services.Transformation;
using HBD.Services.Transformation.TokenExtractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HBD.Services.Email.Templates;

namespace HBD.Services.Email.Providers
{
    public class MailMessageProvider : IMailMessageProvider
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly ITransformer _transformer;

        /// <summary>
        /// Use default ITransformer and DefaultTokenExtractors except AngledBracketTokenExtractor
        /// </summary>
        /// <param name="emailTemplateProvider"></param>
        //public MailMessageProvider(IEmailTemplateProvider emailTemplateProvider)
        //: this(emailTemplateProvider, Transformer.DefaultTokenExtractors
        //      .Where(e => !(e is AngledBracketTokenExtractor)))
        //{
        //}

        /// <summary>
        /// Use default ITransformer with custom tokenExtractors
        /// </summary>
        /// <param name="emailTemplateProvider"></param>
        /// <param name="tokenExtractors"></param>
        //public MailMessageProvider(IEmailTemplateProvider emailTemplateProvider, IEnumerable<ITokenExtractor> tokenExtractors)
        //    : this(emailTemplateProvider, new Transformer(op => op.TokenExtractors
        //        .AddRange(tokenExtractors)))
        //{
        //}

        /// <summary>
        /// Use custom ITransformer
        /// </summary>
        /// <param name="emailTemplateProvider"></param>
        /// <param name="transformer"></param>
        public MailMessageProvider(IEmailTemplateProvider emailTemplateProvider, ITransformer transformer)
        {
            _emailTemplateProvider = emailTemplateProvider ?? throw new ArgumentNullException(nameof(emailTemplateProvider));
            this._transformer = transformer ?? throw new ArgumentNullException(nameof(transformer));
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

        public virtual async Task<MailMessage> GetMailMessageAsync(string templateName, object[] transformData,
            params string[] attachments)

        {
            var template = await _emailTemplateProvider.GetTemplate(templateName).ConfigureAwait(false);
            if (template == null)
                throw new TemplateNotFoundException(templateName);

            return await GetMailMessageAsync(template, transformData, attachments).ConfigureAwait(false);
        }

        public void Dispose() => _emailTemplateProvider?.Dispose();
    }
}
