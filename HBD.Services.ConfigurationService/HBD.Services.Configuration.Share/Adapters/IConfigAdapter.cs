using System;

namespace HBD.Services.Configuration.Adapters
{
#if NETSTANDARD2_0 || NETSTANDARD1_6
    /// <summary>
    /// The adapter for IConfigurationService
    /// To export this adapter to Mef by using below Export pattern.
    /// [Export(typeof(IConfigAdapter)), Shared]
    /// </summary>
#else
    /// <summary>
    /// The adapter for IConfigurationService
    /// To export this adapter to Mef by using below Export pattern.
    ///  [Export(typeof(IConfigAdapter)), PartCreationPolicy(CreationPolicy.Shared)]
    /// </summary>
#endif
    public interface IConfigAdapter
    {
        /// <summary>
        /// Check whether the config in storage has changed or not.
        /// If the cheing is not available then just return false.
        /// The configuration will be re-load whenever the Expiration met.
        /// </summary>
        /// <returns></returns>
        bool IsChanged();

        /// <summary>
        /// The cahing expiration of this configuration.
        /// </summary>
        TimeSpan Expiration { get; }
    }

    public interface IConfigAdapter<out TConfig> : IConfigAdapter where TConfig : class
    {
        /// <summary>
        /// Load TConfig from storage.
        /// </summary>
        /// <returns></returns>
        TConfig Load();
    }
}
