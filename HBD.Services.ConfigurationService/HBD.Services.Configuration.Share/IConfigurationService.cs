using System;
using System.Collections.Generic;
using HBD.Services.Configuration.Adapters;

namespace HBD.Services.Configuration
{
    /// <summary>
    /// The configuration service allows to load the config from various source based on the Adapters provided.
    /// </summary>
    public interface IConfigurationService : IDisposable
    {
        /// <summary>
        /// The registerd Adapters
        /// </summary>
        IReadOnlyCollection<IConfigAdapter> Adapters { get; }

        /// <summary>
        /// The the TConfig from adapters.
        /// The first Not Null values will be return.
        /// If there is more than one Adapter for the TConfig, provides filterSelector to filter it. if not the first matched afapter will be picked up.
        /// The Adapters will be load from HBD.Servicelocator. If there is no adapter found the AdapterNotFoundException will be thrown.
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        /// <exception cref="AdapterNotFoundException"> If there is no adapter found for TConfig. The AdapterNotFoundException will be thrown.</exception>
        TConfig Get<TConfig>(Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class;

        /// <summary>
        /// Manual Adapter registration.
        /// </summary>
        /// <param name="adapter"></param>
        void Register(IConfigAdapter adapter);

        /// <summary>
        /// Remove an registered adapter.
        /// </summary>
        /// <param name="adapter">Registerd adapter.</param>
        void Remove(IConfigAdapter adapter);

        /// <summary>
        /// Remove an registered adapter.
        /// </summary>
        /// <typeparam name="TAdapter">Adapter type</typeparam>
        void Remove<TAdapter>() where TAdapter : IConfigAdapter;
    }
}