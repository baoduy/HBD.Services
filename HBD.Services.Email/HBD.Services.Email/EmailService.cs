using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HBD.Services.Email.Configurations;
using HBD.Services.Email.Exceptions;

namespace HBD.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly object _locker = new object();
        private bool _initialized = false;

        private readonly Action<EmailOptions> _optionFactory;
        private SmtpClient _smtpClient;
        private MailAddress _fromEmail;

        private IReadOnlyCollection<IEmailInfo> _emailInfos;
        public IReadOnlyCollection<IEmailInfo> EmailInfos
        {
            get
            {
                EnsureInitialized();
                return _emailInfos;
            }
        }

        public EmailService(Action<EmailOptions> optionFactory)
            => this._optionFactory = optionFactory ?? throw new ArgumentNullException(nameof(optionFactory));

        private void EnsureInitialized()
        {
            if (_initialized) return;
            lock (_locker)
            {
                var options = new EmailOptions();
                _optionFactory.Invoke(options);

                if (options.SmtpClientFactory == null)
                    throw new ArgumentNullException(nameof(options.SmtpClientFactory));

                _fromEmail = options.FromEmailAddress;

#if !NETSTANDARD2_0
                if (_fromEmail == null)
                    _fromEmail = Extensions.GetDefaultFromEmail();
#endif

                if (_fromEmail == null)
                    throw new ArgumentNullException(nameof(options.FromEmailAddress));

                _smtpClient = options.SmtpClientFactory.Invoke();

                var list = options.GetEmailTemplates();

                var duplicated = list.GetDuplicated();

                if (duplicated.Any())
                    throw new TemplateDuplicatedException(string.Join(",", duplicated));

                this._emailInfos = new ReadOnlyCollection<IEmailInfo>(list.Select(t => new EmailInfo(t)).ToArray());

                _initialized = true;
            }
        }

        private IEmailInfo GetEmailInfo(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                throw new ArgumentNullException(nameof(templateName));

            EnsureInitialized();

            var template = EmailInfos.FirstOrDefault(i => i.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase));
            if (template == null)
                throw new TemplateNotFoundException(templateName);

            return template;
        }


        private MailMessage ConsolidateEmail(MailMessage mailMessage, params string[] attachments)
        {
            mailMessage.From = _fromEmail;

            foreach (var attachment in attachments)
                mailMessage.Attachments.Add(new Attachment(attachment));

            return mailMessage;
        }

        public async Task<MailMessage> GetMailMessageAsync(string templateName, object[] transformData, params string[] attachments)
        {
            var template = GetEmailInfo(templateName);
            var mail = await template.ToMailMessageAsync(transformData);

            return ConsolidateEmail(mail);
        }

        public MailMessage GetMailMessage(string templateName, object[] transformData, params string[] attachments)
        {
            var template = GetEmailInfo(templateName);
            var mail = template.ToMailMessage(transformData);

            return ConsolidateEmail(mail);
        }

        public void Send(string templateName, object[] transformData, params string[] attachments)
        {
            var email = GetMailMessage(templateName, transformData, attachments);
            this.Send(email);
        }

        public void Send(MailMessage email)
        {
            EnsureInitialized();
            _smtpClient.Send(ConsolidateEmail(email));
        }

        public Task SendAsync(string templateName, object[] transformData, params string[] attachments)
        {
            var email = GetMailMessageAsync(templateName, transformData, attachments).Result;
            return this.SendAsync(email);
        }

        public async Task SendAsync(MailMessage email)
        {
            EnsureInitialized();
            await _smtpClient.SendMailAsync(ConsolidateEmail(email));
        }

        public virtual void Dispose() => _smtpClient?.Dispose();
    }
}
