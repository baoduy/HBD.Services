using System;
using HBD.Services.Email.Configurations;

namespace HBD.Services.Email.Exceptions
{
    public class InvalidTemplateException : Exception
    {
        public InvalidTemplateException(EmailTemplate template):base($"The template {template.Name} is invalid.")
        {
            Template = template;
        }

        public EmailTemplate Template { get; }
    }
}
