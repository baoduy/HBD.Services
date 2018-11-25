using HBD.Services.Configuration.Adapters;
using System;

namespace HBD.Services.Configuration.Exceptions
{
    public class AdapterRegisterdException : Exception
    {
        public AdapterRegisterdException(IConfigAdapter adapter)
            : this(adapter.GetType())
        { }

        public AdapterRegisterdException(Type adapterType)
            : base($"The adapter {adapterType.FullName} is already registered.")
        { }
    }
}
