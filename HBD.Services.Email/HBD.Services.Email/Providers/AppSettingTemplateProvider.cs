using HBD.Services.Email.Templates;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.Services.Email.Providers
{
    public class AppSettingTemplateProvider : EmailTemplateProvider
    {
        #region Fields

        private IOptions<EmailTemplateSection> _options;

        #endregion Fields

        #region Constructors

        public AppSettingTemplateProvider(IOptions<EmailTemplateSection> options) => _options = options;

        #endregion Constructors

        #region Methods

        protected override Task<IEnumerable<EmailTemplate>> LoadTemplatesAsync()
            => Task.FromResult((IEnumerable<EmailTemplate>)_options.Value.Templates);

        #endregion Methods
    }
}