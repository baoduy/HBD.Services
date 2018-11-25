using System;
using System.Threading.Tasks;
using HBD.Services.Email.Templates;

namespace HBD.Services.Email.Providers
{
    public interface IEmailTemplateProvider : IDisposable
    {
        Task<IEmailTemplate> GetTemplate(string templateName);
    }
}
