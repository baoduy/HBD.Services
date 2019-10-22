using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using HBD.Services.Email.Builders;
using HBD.Services.Email.Templates;

namespace HBD.Services.Email.Providers
{
    public class InlineEmailTemplateProvider : IEmailTemplateProvider
    {
        IDictionary<string, IEmailTemplate> _templates;

        public InlineEmailTemplateProvider(Action<IEmailTemplateBuilder> actions)
        {
            if (actions is null)
                throw new ArgumentNullException(nameof(actions));

            _templates = new Dictionary<string, IEmailTemplate>();
            var builder = new EmailTemplateBuilder(_templates);
            actions.Invoke(builder);
            //Validate Templates
            builder.Build();

        }

        public void Dispose() { }

        public Task<IEmailTemplate> GetTemplate(string templateName)
        {
            var name = templateName.ToUpper();

            if (_templates.ContainsKey(name))
                return Task.FromResult(_templates[name]);

            return Task.FromResult<IEmailTemplate>(null);
        }
    }
}
