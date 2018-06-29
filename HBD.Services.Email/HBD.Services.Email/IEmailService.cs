using System;
using System.Threading.Tasks;
using HBD.Services.Email.Exceptions;

namespace HBD.Services.Email
{
    public interface IEmailService:IDisposable
    {
        /// <summary>
        /// Send email by using predefined template.
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="transformData"></param>
        /// <param name="attachements"></param>
        /// <exception cref="TemplateNotFoundException">Template not found</exception>
        /// <exception cref="ArgumentNullException">when templateName is null</exception>
        void Send(string templateName, object[] transformData, params string[] attachements);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="transformData"></param>
        /// <param name="attachements"></param>
        /// <exception cref="TemplateNotFoundException">Template not found</exception>
        /// <exception cref="ArgumentNullException">when templateName is null</exception>
        /// <returns></returns>
        Task SendAsync(string templateName, object[] transformData, params string[] attachements);

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentNullException">when email is null</exception>
        /// <returns></returns>
        void Send(System.Net.Mail.MailMessage email);

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentNullException">when email is null</exception>
        /// <returns></returns>
        Task SendAsync(System.Net.Mail.MailMessage email);
    }
}
