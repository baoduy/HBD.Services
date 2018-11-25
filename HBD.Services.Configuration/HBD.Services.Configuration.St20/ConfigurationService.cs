using HBD.Services.Configuration.Adapters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private bool _isDisposed;
        private readonly List<IConfigAdapter> _adapters;
        private readonly IMemoryCache _cacheProvider;
        private readonly TimeSpan _defaultExpiration;
        private readonly bool _ignoreCaching;

        public IReadOnlyCollection<IConfigAdapter> Adapters => _adapters;

        public ConfigurationService(ConfigurationOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _defaultExpiration = options.DefaultExpiration ?? new TimeSpan(4, 0, 0);
            _cacheProvider = options.CacheProvider;
            _adapters = new List<IConfigAdapter>();
            _ignoreCaching = options.IsIgnoreCaching;
            _adapters.AddRange(options.Adapters);
        }

        private IConfigAdapter<TConfig> GetAdapter<TConfig>() where TConfig : class => _adapters.OfType<IConfigAdapter<TConfig>>().FirstOrDefault();

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
        {
            _cacheProvider?.Dispose();
            _adapters.Clear();
            _isDisposed = isDisposing;
        }

        private TConfig TryGetFromCache<TConfig>()
        {
            var cacheKey = typeof(TConfig).FullName;
            return _cacheProvider != null ? _cacheProvider.Get<TConfig>(cacheKey) : default(TConfig);
        }

        private void SetToCache<TConfig>(IConfigAdapter adapter, TConfig config)
        {
            if (_ignoreCaching || _cacheProvider == null) return;

            var cacheKey = typeof(TConfig).FullName;

            var t = adapter.Expiration ?? _defaultExpiration;
            if (t <= TimeSpan.MinValue)
                t = _defaultExpiration;

            _cacheProvider.Set(cacheKey, config, t);
        }


        public async Task<TConfig> GetAsync<TConfig>() where TConfig : class
        {
            CheckDisposed();

            var adapter = GetAdapter<TConfig>();

            //1. Load from cache
            var val = TryGetFromCache<TConfig>();

            //2. Check if value has changed set val is Null to reload it.
            if (val != null && !adapter.HasChanged())
                return val;

            val = await adapter.LoadAsync().ConfigureAwait(false);

            SetToCache<TConfig>(adapter, val);

            return val;
        }
    }
}
