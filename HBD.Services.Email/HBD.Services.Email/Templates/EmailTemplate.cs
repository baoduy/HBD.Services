using Newtonsoft.Json;
using System.IO;

namespace HBD.Services.Email.Templates
{
    public class EmailTemplate : IEmailTemplate
    {
        #region Properties
        public EmailTemplate() { }

        [JsonConstructor]
        public EmailTemplate([JsonProperty(nameof(Name))]string name) => Name = name;

        /// <inheritdoc />
        public string BccEmails { get; set; }

        /// <inheritdoc />
        public string Body { get; set; }

        /// <inheritdoc />
        public string BodyFile { get; set; }

        /// <inheritdoc />
        public string CcEmails { get; set; }

        /// <inheritdoc />
        public bool IsBodyHtml { get; set; } = true;

        public bool IsValid
        {
            get
            {
                var emailValid = !string.IsNullOrWhiteSpace(ToEmails) || !string.IsNullOrWhiteSpace(BccEmails) || !string.IsNullOrWhiteSpace(CcEmails);

                if (string.IsNullOrWhiteSpace(BodyFile))
                    return emailValid && !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(Subject);

                return emailValid && !string.IsNullOrWhiteSpace(Subject) && File.Exists(BodyFile);
            }
        }

        /// <inheritdoc />
        [JsonRequired]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonRequired]
        public string Subject { get; set; }

        /// <inheritdoc />
        public string ToEmails { get; set; }

        #endregion Properties
    }
}