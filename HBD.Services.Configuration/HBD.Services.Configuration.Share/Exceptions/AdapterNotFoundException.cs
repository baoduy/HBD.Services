using System;

namespace HBD.Services.Configuration.Exceptions
{
    public class AdapterNotFoundException : Exception
    {
        public AdapterNotFoundException(Type configType)
            : base($"The adapter for {configType.FullName} is not found.")
        { }
    }
}
