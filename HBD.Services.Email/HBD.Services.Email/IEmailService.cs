using HBD.Services.Email.Exceptions;
using System;
using System.Threading.Tasks;

namespace HBD.Services.Email
{
    public interface IEmailService : IDisposable
    {
        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="transformData"></param>
        /// <param name="attachments"></param>
        /// <exception cref="TemplateNotFoundException">Template not found</exception>
        /// <exception cref="ArgumentNullException">when templateName is null</exception>
        /// <returns></returns>
        Task SendAsync(string templateName, object[] transformData, params string[] attachments);

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentNullException">when email is null</exception>
        /// <returns></returns>
        Task SendAsync(System.Net.Mail.MailMessage email);

        #endregion Methods
    }
}