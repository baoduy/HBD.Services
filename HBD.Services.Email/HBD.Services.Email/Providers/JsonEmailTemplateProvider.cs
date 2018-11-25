using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HBD.Services.Email.Exceptions;
using HBD.Services.Email.Templates;
using Newtonsoft.Json;

namespace HBD.Services.Email.Providers
{
    public class JsonEmailTemplateProvider : IEmailTemplateProvider
    {
        private bool _initialized = false;
        private readonly string _configFile;

        public IReadOnlyCollection<IEmailTemplate> Collection { get; private set; }

        public JsonEmailTemplateProvider(string configFile) => _configFile = configFile;

        public async Task<IEmailTemplate> GetTemplate(string templateName)
        {
            await EnsureInitialized().ConfigureAwait(false);
            return Collection.FirstOrDefault(t => t.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase));
        }

        private static async Task<string> ReadToAsync(string file)
        {
            using (var reader = File.OpenText(file))
                return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        private async Task EnsureInitialized()
        {
            if (_initialized) return;

            if (!File.Exists(_configFile))
                throw new FileNotFoundException(_configFile);

            var fileText = await ReadToAsync(_configFile);

            if (string.IsNullOrWhiteSpace(fileText))
                throw new InvalidDataException(_configFile);

            var templates = JsonConvert.DeserializeObject<EmailTemplate[]>(fileText);

            //Reading Body
            foreach (var template in templates)
            {
                if (!string.IsNullOrEmpty(template.BodyFile) && !File.Exists(template.BodyFile))
                    template.BodyFile = Path.Combine(Path.GetDirectoryName(_configFile) ?? AppDomain.CurrentDomain.BaseDirectory, template.BodyFile);

                if (!template.IsValid)
                    throw new InvalidTemplateException(template);

                if (!string.IsNullOrEmpty(template.BodyFile))
                    template.Body = await ReadToAsync(template.BodyFile);
            }

            Collection = new ReadOnlyCollection<IEmailTemplate>(new List<IEmailTemplate>(templates));

            _initialized = true;
        }

        public void Dispose()
        {

        }


    }
}
