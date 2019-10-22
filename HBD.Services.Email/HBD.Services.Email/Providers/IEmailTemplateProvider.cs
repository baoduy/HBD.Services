using HBD.Services.Email.Templates;
using System;
using System.Threading.Tasks;

namespace HBD.Services.Email.Providers
{
    public interface IEmailTemplateProvider : IDisposable
    {
        #region Methods

        Task<IEmailTemplate> GetTemplate(string templateName);

        #endregion Methods
    }
}