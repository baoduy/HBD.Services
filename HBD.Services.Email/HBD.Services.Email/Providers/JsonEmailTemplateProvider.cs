using System.Collections.Generic;
using System.IO;
using HBD.Services.Email.Templates;
using Newtonsoft.Json;

namespace HBD.Services.Email.Providers
{
    public class JsonEmailTemplateProvider : EmailTemplateProvider
    {
        #region Fields

        private readonly string _configFile;


        #endregion Fields

        #region Constructors

        public JsonEmailTemplateProvider(string configFile)
        {
            _configFile = Path.GetFullPath(configFile);
        }

        #endregion Constructors

        #region Properties

        protected override async System.Threading.Tasks.Task<IEnumerable<EmailTemplate>> LoadTemplatesAsync()
        {
            if (!File.Exists(_configFile))
                throw new FileNotFoundException(_configFile);

            var fileText = await ReadToAsync(_configFile);

            if (string.IsNullOrWhiteSpace(fileText))
                throw new InvalidDataException(_configFile);

            return JsonConvert.DeserializeObject<EmailTemplate[]>(fileText);
        }

        #endregion Properties

        #region Methods







        #endregion Methods
    }
}