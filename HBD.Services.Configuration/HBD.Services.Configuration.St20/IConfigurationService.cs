﻿using HBD.Services.Configuration.Adapters;
using HBD.Services.Configuration.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.Services.Configuration
{
    /// <summary>
    /// The configuration service allows to load the config from various source based on the Adapters provided.
    /// </summary>
    public interface IConfigurationService : IDisposable
    {
        #region Properties

        /// <summary>
        /// The registered Adapters
        /// </summary>
        IReadOnlyCollection<IConfigAdapter> Adapters { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Load the TConfig from adapters.
        /// The first Not Null values will be return.
        /// If there is more than one Adapter for the TConfig, provides filterSelector to filter it. if not the first matched adapter will be picked up.
        /// The Adapters will be load from HBD.Service-locator. If there is no adapter found the AdapterNotFoundException will be thrown.
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <returns></returns>
        /// <exception cref="AdapterNotFoundException"> If there is no adapter found for TConfig. The AdapterNotFoundException will be thrown.</exception>
        Task<TConfig> GetAsync<TConfig>() where TConfig : class;

        #endregion Methods
    }
}