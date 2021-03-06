﻿using HBD.Services.Email.Exceptions;
using HBD.Services.Email.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HBD.Services.Email.Providers
{
    public abstract class EmailTemplateProvider : IEmailTemplateProvider
    {
        #region Fields

        private bool _initialized;

        #endregion Fields

        #region Constructors

        protected EmailTemplateProvider() => Templates = new Dictionary<string, IEmailTemplate>();

        #endregion Constructors

        #region Properties

        protected IDictionary<string, IEmailTemplate> Templates { get; private set; }

        #endregion Properties

        #region Methods

        public virtual void Dispose()
        {
        }

        public async Task<IEmailTemplate> GetTemplate(string templateName)
        {
            await EnsureInitialized().ConfigureAwait(false);
            var name = templateName.ToUpper();

            if (Templates.ContainsKey(name))
                return Templates[name];
            return null;
        }

        protected static async Task<string> ReadToAsync(string file)
        {
            using (var reader = File.OpenText(file))
                return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        protected abstract Task<IEnumerable<EmailTemplate>> LoadTemplatesAsync();

        private async Task EnsureInitialized()
        {
            if (_initialized) return;

            var templates = await LoadTemplatesAsync();

            //Reading Body
            foreach (var template in templates)
            {
                if (!string.IsNullOrEmpty(template.BodyFile) && !File.Exists(template.BodyFile))
                    template.BodyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template.BodyFile);

                if (!template.IsValid)
                    throw new InvalidTemplateException(template);

                if (!string.IsNullOrEmpty(template.BodyFile))
                    template.Body = await ReadToAsync(template.BodyFile);

                Templates.Add(template.Name.ToUpper(), template);
            }

            _initialized = true;
        }

        #endregion Methods
    }
}