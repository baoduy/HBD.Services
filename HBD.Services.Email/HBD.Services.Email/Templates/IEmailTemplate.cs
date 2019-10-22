namespace HBD.Services.Email.Templates
{
    public interface IEmailTemplate
    {
        #region Properties

        /// <summary>
        /// Bcc Emails separate by comma (,).
        /// The template will be extract from Data later.
        /// </summary>
        string BccEmails { get; set; }

        /// <summary>
        /// The email Body.
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// The email Body file, it can be a html file. The file content will be loaded to Body before send email out.
        /// </summary>
        string BodyFile { get; set; }

        /// <summary>
        /// Cc Emails temperate by comma (,).
        /// The template will be extract from Data later.
        /// </summary>
        string CcEmails { get; set; }

        /// <summary>
        /// Indicate the Body is in Html format or not
        /// </summary>
        bool IsBodyHtml { get; set; }

        /// <summary>
        /// The name of Template
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The email Subject
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// To Emails separate by comma (,).
        /// The template will be extract from Data later.
        /// </summary>
        string ToEmails { get; set; }

        #endregion Properties
    }
}