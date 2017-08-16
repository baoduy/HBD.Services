using System;
using System.Threading.Tasks;

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
        /// If the change tracking is not available then just return false.
        /// The configuration will be re-load whenever the Expiration met.
        /// </summary>
        /// <returns></returns>
        bool IsChanged();

        /// <summary>
        /// The caching expiration of this configuration.
        /// If Expiration is Null the default expiration of CacheService will be used.
        /// </summary>
        TimeSpan? Expiration { get; }
    }

    public interface IConfigAdapter<TConfig> : IConfigAdapter where TConfig : class
    {
        /// <summary>
        /// Load TConfig from storage.
        /// </summary>
        /// <returns></returns>
        TConfig Load();

        /// <summary>
        /// Load TConfig from storage.
        /// </summary>
        /// <returns></returns>
        Task<TConfig> LoadAsync();

        /// <summary>
        /// Save the config changes back to the storage.
        /// </summary>
        /// <param name="config"></param>
        void Save(TConfig config);

        /// Save the config changes back to the storage.
        /// </summary>
        /// <param name="config"></param>
        Task SaveAsync(TConfig config);
    }
}
