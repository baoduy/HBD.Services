using System;
using System.Net.Mail;
using System.Threading.Tasks;
using HBD.Services.Email.Providers;
using HBD.Services.Email.Templates;

namespace HBD.Services.Email
{
    public class EmailService : IEmailService
    {
        private bool _initialized = false;
        private readonly IMailMessageProvider _mailMessageProvider;
        private SmtpClient _smtpClient;
        private MailAddress _fromEmail;
        private readonly EmailOptions _options;

        public EmailService(IMailMessageProvider mailMessageProvider)
            : this(mailMessageProvider, null)
        {
        }

        public EmailService(IMailMessageProvider mailMessageProvider, EmailOptions options)
        {
            _mailMessageProvider = mailMessageProvider ?? throw new ArgumentNullException(nameof(mailMessageProvider));
            _options = options;
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            _smtpClient = _options?.SmtpClientFactory() ?? new SmtpClient();
            _fromEmail = _options?.FromEmailAddress;

            _initialized = true;
        }

        private MailMessage ConsolidateEmail(MailMessage mailMessage)
        {
            EnsureInitialized();

            if (mailMessage.From == null)
                mailMessage.From = _fromEmail;

            if (mailMessage.From == null)
                throw new ArgumentException(nameof(mailMessage.From));

            return mailMessage;
        }

        public virtual async Task SendAsync(string templateName, object[] transformData, params string[] attachments)
        {
            var email = await _mailMessageProvider.GetMailMessageAsync(templateName, transformData, attachments)
                .ConfigureAwait(false);

            await this.SendAsync(email).ConfigureAwait(false);
        }

        public virtual Task SendAsync(MailMessage email)
        {
            EnsureInitialized();
            return _smtpClient.SendMailAsync(ConsolidateEmail(email));
        }

        public void Dispose()
        {
           _smtpClient?.Dispose();
            _fromEmail = null;
            _mailMessageProvider?.Dispose();
        }
    }
}
