using System.IO;
using Newtonsoft.Json;

namespace HBD.Services.Email.Configurations
{
    public class EmailTemplate
    {
        /// <summary>
        /// If loading frol Json file.
        /// This Property will be setted for later using to load BodyFile.
        /// </summary>
        internal string FromJsonFile { get; set; }

        [JsonConstructor]
        public EmailTemplate([JsonProperty(nameof(Name))]string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of Template
        /// </summary>
        [JsonRequired]
        public string Name { get; }

        /// <summary>
        /// To Emails seperate by comma (,). The template name format is [EmailProperty].
        /// The template will be extract from Data later.
        /// </summary>
        public string ToEmails { get; set; }

        /// <summary>
        /// Cc Emails seperate by comma (,). The template name format is [EmailProperty].
        /// The template will be extract from Data later.
        /// </summary>
        public string CcEmails { get; set; }

        /// <summary>
        /// Bcc Emails seperate by comma (,). The template name format is [EmailProperty].
        /// The template will be extract from Data later.
        /// </summary>
        public string BccEmails { get; set; }

        /// <summary>
        /// The email Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The email Body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The email Body file, it can be a html file. The file content will be loaded to Body before send email out.
        /// </summary>
        public string BodyFile { get; set; }

        /// <summary>
        /// Indicate the Body is in Html format or not
        /// </summary>
        public bool IsBodyHtml { get; set; } = true;

        public bool IsValid
        {
            get
            {
                var emailValid =!string.IsNullOrWhiteSpace( ToEmails) || !string.IsNullOrWhiteSpace(BccEmails) || !string.IsNullOrWhiteSpace(CcEmails);

                if (string.IsNullOrWhiteSpace(BodyFile))
                    return emailValid && !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(Subject);

                var file = GetBodyFile();
                return emailValid  && !string.IsNullOrWhiteSpace(Subject) && File.Exists(file);
            }
        }

        internal string GetBodyFile()
        {
            if (File.Exists(BodyFile))
                return BodyFile;

            return !string.IsNullOrWhiteSpace( FromJsonFile) 
                ? Path.Combine(Path.GetDirectoryName(FromJsonFile), BodyFile) 
                : BodyFile;
        }
    }
}
