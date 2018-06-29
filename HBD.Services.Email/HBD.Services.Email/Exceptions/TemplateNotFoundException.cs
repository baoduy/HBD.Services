using System;

namespace HBD.Services.Email.Exceptions
{
    public sealed class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException()
        {
        }

        public TemplateNotFoundException(string message) : base(message)
        {
        }

        public TemplateNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
