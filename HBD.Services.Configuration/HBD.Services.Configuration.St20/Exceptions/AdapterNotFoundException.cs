using System;

namespace HBD.Services.Configuration.Exceptions
{
    public class AdapterNotFoundException : Exception
    {
        #region Constructors

        public AdapterNotFoundException(Type configType)
            : base($"The adapter for {configType.FullName} is not found.")
        { }

        #endregion Constructors
    }
}