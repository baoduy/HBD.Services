using System;

namespace HBD.Services.Email.Exceptions
{
    public sealed class TemplateDuplicatedException : Exception
    {
        public TemplateDuplicatedException(string name) : this(name, null)
        {
        }

        public TemplateDuplicatedException(string name, Exception innerException) : base($"Template {name} are duplicated.", innerException)
        {
        }
    }
}
