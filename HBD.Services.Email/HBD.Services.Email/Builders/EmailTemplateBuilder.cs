using HBD.Services.Email.Exceptions;
using HBD.Services.Email.Templates;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace HBD.Services.Email.Builders
{
    public interface IDestinationBuilder
    {
        #region Methods

        IDestinationBuilder Bcc(params string[] emails);

        IDestinationBuilder Cc(params string[] emails);

        IDestinationBuilder To(params string[] emails);

        IEmailBuilder With();

        #endregion Methods
    }

    public interface IEmailBuilder
    {
        #region Methods

        /// <summary>
        /// Add more template
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDestinationBuilder Add(string name);

        /// <summary>
        /// Non Html body
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        IEmailBuilder Body(string body);

        /// <summary>
        /// Load body from Html file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IEmailBuilder BodyFrom(string file);

        /// <summary>
        /// Build Templates
        /// </summary>
        /// <returns></returns>
        ICollection<IEmailTemplate> Build();

        IEmailBuilder Subject(string subject);

        #endregion Methods
    }

    public interface IEmailTemplateBuilder
    {
        #region Methods

        IDestinationBuilder Add(string name);

        ICollection<IEmailTemplate> Build();

        #endregion Methods
    }

    public class EmailTemplateBuilder : IEmailTemplateBuilder, IDestinationBuilder, IEmailBuilder
    {
        #region Fields

        private EmailTemplate _current;

        private IDictionary<string, IEmailTemplate> _templates;

        #endregion Fields

        #region Constructors

        internal EmailTemplateBuilder(IDictionary<string, IEmailTemplate> container = null) => _templates = container ?? new Dictionary<string, IEmailTemplate>();

        #endregion Constructors

        #region Methods

        public static IDestinationBuilder New(string name) => new EmailTemplateBuilder().Add(name);

        public IDestinationBuilder Add(string name)
        {
            if (_current != null && !_current.IsValid)
                throw new InvalidTemplateException(_current);

            _current = new EmailTemplate(name);
            _templates.Add(_current.Name.ToUpper(), _current);

            return this;
        }

        public IDestinationBuilder Bcc(params string[] emails)
        {
            Contract.Requires(emails.Length > 0);
            _current.BccEmails = string.Join(",", emails);
            return this;
        }

        public IEmailBuilder Body(string body)
        {
            if (string.IsNullOrEmpty(body)) throw new ArgumentNullException(nameof(body));
            _current.Body = body;
            _current.IsBodyHtml = false;
            return this;
        }

        public IEmailBuilder BodyFrom(string file)
        {
            if (!System.IO.File.Exists(file)) throw new ArgumentNullException(nameof(file));
            _current.BodyFile = file;
            _current.IsBodyHtml = true;
            return this;
        }

        public ICollection<IEmailTemplate> Build()
        {
            var invalid = _templates.Values.OfType<EmailTemplate>().FirstOrDefault(v => !v.IsValid);

            if (invalid != null)
                throw new InvalidTemplateException(invalid);

            return _templates.Values;
        }

        public IDestinationBuilder Cc(params string[] emails)
        {
            Contract.Requires(emails.Length > 0);
            _current.CcEmails = string.Join(",", emails);
            return this;
        }

        public IEmailBuilder Subject(string subject)
        {
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));
            _current.Subject = subject;

            return this;
        }

        public IDestinationBuilder To(params string[] emails)
        {
            Contract.Requires(emails.Length > 0);
            _current.ToEmails = string.Join(",", emails);
            return this;
        }

        public IEmailBuilder With() => this;

        #endregion Methods
    }
}