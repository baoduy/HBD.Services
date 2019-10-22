using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HBD.Services.Email.Providers
{
    public interface IMailMessageProvider : IDisposable
    {
        #region Methods

        Task<MailMessage> GetMailMessageAsync(string templateName, object[] transformData, params string[] attachments);

        #endregion Methods
    }
}