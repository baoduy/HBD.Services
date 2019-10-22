using HBD.Services.Email.Exceptions;
using HBD.Services.Email.Providers;
using HBD.Services.Email.Templates;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HBD.Services.Email
{
    public class SmtpEmailService : IEmailService
    {
        #region Fields

        private readonly IMailMessageProvider _mailMessageProvider;
        private readonly SmtpEmailOptions _options;
        private MailAddress _fromEmail;
        private bool _initialized;
        private SmtpClient _smtpClient;

        #endregion Fields

        #region Constructors

        public SmtpEmailService(IMailMessageProvider mailMessageProvider)
            : this(mailMessageProvider, null)
        {
        }

        public SmtpEmailService(IMailMessageProvider mailMessageProvider, SmtpEmailOptions options)
        {
            _mailMessageProvider = mailMessageProvider ?? throw new ArgumentNullException(nameof(mailMessageProvider));
            _options = options;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            _smtpClient?.Dispose();
            _fromEmail = null;
        }

        public virtual async Task SendAsync(string templateName, object[] transformData, params string[] attachments)
        {
            var email = await _mailMessageProvider.GetMailMessageAsync(templateName, transformData, attachments)
                .ConfigureAwait(false);

            if (email == null)
                throw new TemplateNotFoundException(templateName);

            await this.SendAsync(email).ConfigureAwait(false);
        }

        public virtual Task SendAsync(MailMessage email)
        {
            EnsureInitialized();
            return _smtpClient.SendMailAsync(ConsolidateEmail(email));
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

        private void EnsureInitialized()
        {
            if (_initialized) return;

            _smtpClient = _options?.SmtpClientFactory() ?? new SmtpClient();
            _fromEmail = _options?.FromEmailAddress;

            _initialized = true;
        }

        #endregion Methods
    }
}