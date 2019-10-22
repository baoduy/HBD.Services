using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.Services.Email.Templates;
using Microsoft.Extensions.Options;

namespace HBD.Services.Email.Providers
{
    public class AppSettingTemplateProvider : EmailTemplateProvider
    {
        IOptions<EmailTemplateSection> _options;

        public AppSettingTemplateProvider(IOptions<EmailTemplateSection> options) => _options = options;

        protected override Task<IEnumerable<EmailTemplate>> LoadTemplatesAsync()
            => Task.FromResult((IEnumerable<EmailTemplate>)_options.Value.Templates);
    }
}
