using System;
using System.Collections.Generic;
using HBD.Services.Configuration.Adapters;
using System.Threading.Tasks;

namespace HBD.Services.Configuration
{
    /// <summary>
    /// The configuration service allows to load the config from various source based on the Adapters provided.
    /// </summary>
    public interface IConfigurationService : IDisposable
    {
        /// <summary>
        /// The registered Adapters
        /// </summary>
        IReadOnlyCollection<IConfigAdapter> Adapters { get; }

        /// <summary>
        /// Load the TConfig from adapters.
        /// The first Not Null values will be return.
        /// If there is more than one Adapter for the TConfig, provides filterSelector to filter it. if not the first matched afapter will be picked up.
        /// The Adapters will be load from HBD.Servicelocator. If there is no adapter found the AdapterNotFoundException will be thrown.
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        /// <exception cref="AdapterNotFoundException"> If there is no adapter found for TConfig. The AdapterNotFoundException will be thrown.</exception>
        TConfig Get<TConfig>(Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class;

        /// <summary>
        /// Load the TConfig from adapters.
        /// The first Not Null values will be return.
        /// If there is more than one Adapter for the TConfig, provides filterSelector to filter it. if not the first matched afapter will be picked up.
        /// The Adapters will be load from HBD.Servicelocator. If there is no adapter found the AdapterNotFoundException will be thrown.
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        /// <exception cref="AdapterNotFoundException"> If there is no adapter found for TConfig. The AdapterNotFoundException will be thrown.</exception>
        Task<TConfig> GetAsync<TConfig>(Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class;

        /// <summary>
        /// Save changes to storage
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="config"></param>
        void Save<TConfig>(TConfig config, Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class;

        /// <summary>
        /// Save changes to storage
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="config"></param>
        Task SaveAsync<TConfig>(TConfig config, Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class;

        /// <summary>
        /// Manual Adapter registration.
        /// </summary>
        /// <param name="adapter"></param>
        void Register(IConfigAdapter adapter);

        /// <summary>
        /// Remove an registered adapter.
        /// </summary>
        /// <param name="adapter">Registered adapter.</param>
        void Remove(IConfigAdapter adapter);

        /// <summary>
        /// Remove an registered adapter.
        /// </summary>
        /// <typeparam name="TAdapter">Adapter type</typeparam>
        void Remove<TAdapter>() where TAdapter : IConfigAdapter;
    }
}