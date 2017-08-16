using HBD.Services.Configuration.Adapters;
using HBD.Services.Configuration.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HBD.Services.Caching.Providers;
using HBD.Framework.Core;
using HBD.Framework.Attributes;
using System.Threading.Tasks;

namespace HBD.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly object _locker = new object();
        private bool _isInitialized;
        private bool _isDisposed;
        private readonly List<IConfigAdapter> _adapters;
        private readonly IReadOnlyCollection<IConfigAdapter> _readOnlyAdapters;
        private readonly ICacheProvider _cacheProvider;
        private readonly TimeSpan _defaultExpiration;
        private readonly IServiceLocator _serviceLocator;
        private readonly bool _ignoreCaching;
        private readonly bool _ignoreServiceLocator;

        public IReadOnlyCollection<IConfigAdapter> Adapters
        {
            get
            {
                Initialize();
                return _readOnlyAdapters;
            }
        }

        public ConfigurationService(ConfigurationServiceBuilder configuration = null)
        {

            _defaultExpiration = new TimeSpan(4, 0, 0);
            _cacheProvider = new MemoryCacheProvider(_defaultExpiration);
            _adapters = new List<IConfigAdapter>();
            _readOnlyAdapters = new ReadOnlyCollection<IConfigAdapter>(_adapters);

            if (configuration != null)
            {
                if (configuration.ServiceLocator != null)
                    _serviceLocator = configuration.ServiceLocator;

                if (configuration.DefaultExpiration != null)
                    _defaultExpiration = configuration.DefaultExpiration.Value;

                if (configuration.CacheProvider != null)
                    _cacheProvider = configuration.CacheProvider;

                _ignoreCaching = configuration.IsIgnoreCaching;
                _ignoreServiceLocator = configuration.IsIgnoreServiceLocator;

                _adapters.AddRange(configuration.Adapters);
            }
        }

        private void Initialize()
        {
            lock (_locker)
            {
                if (_isInitialized) return;

                if (!_ignoreServiceLocator)
                {
                    var service = _serviceLocator ?? HBD.ServiceLocator.Current;

                    //Load Adapters form Service location.
                    foreach (var item in service.GetAllInstances<IConfigAdapter>())
                    {
                        if (_adapters.Contains(item)) continue;
                        _adapters.Add(item);
                    }
                }

                _isInitialized = true;
            }
        }

        private IConfigAdapter<TConfig> GetAdapter<TConfig>(Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class
        {
            this.Initialize();

            var adapters = _adapters.OfType<IConfigAdapter<TConfig>>().ToList();

            if (adapters.Count > 0 && filterSelector != null)
                return filterSelector.Invoke(adapters);

            return adapters.FirstOrDefault();
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
        }

        public void Register([NotNull]IConfigAdapter adapter)
        {
            Guard.ArgumentIsNotNull(adapter, nameof(adapter));
            CheckDisposed();

            lock (_locker)
            {
                if (_adapters.Contains(adapter) || _adapters.Any(a => a.GetType() == adapter.GetType()))
                    throw new AdapterRegisterdException(adapter);

                _adapters.Add(adapter);
            }
        }

        public void Remove([NotNull]IConfigAdapter adapter)
        {
            Guard.ArgumentIsNotNull(adapter, nameof(adapter));
            CheckDisposed();

            lock (_locker)
            {
                _adapters.Remove(adapter);
            }
        }

        public void Remove<TAdapter>() where TAdapter : IConfigAdapter
        {
            CheckDisposed();

            lock (_locker)
            {
                var a = _adapters.FirstOrDefault(i => i.GetType() == typeof(TAdapter));
                if (a == null) return;
                _adapters.Remove(a);
            }
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
            return _cacheProvider.Get<TConfig>(cacheKey);
        }

        private void SetToCache<TConfig>(IConfigAdapter adapter, TConfig config)
        {
            if (_ignoreCaching) return;

            var cacheKey = typeof(TConfig).FullName;

            var t = adapter.Expiration ?? _defaultExpiration;
            if (t <= TimeSpan.MinValue)
                t = _defaultExpiration;

            _cacheProvider.Set(cacheKey, config, t);
        }

        public virtual TConfig Get<TConfig>(
            Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null)
            where TConfig : class
        {
            CheckDisposed();

            var adapter = GetAdapter(filterSelector);

            if (adapter == null) return null;

            //1. Load from cache
            var val = TryGetFromCache<TConfig>();

            //2. Check if value has changed set val is Null to reload it.
            if (val != null && !adapter.IsChanged())
                return val;

            val = adapter.Load();

            SetToCache<TConfig>(adapter, val);

            return val;
        }

        public async Task<TConfig> GetAsync<TConfig>(Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class
        {
            CheckDisposed();

            var adapter = GetAdapter(filterSelector);

            //1. Load from cache
            var val = TryGetFromCache<TConfig>();

            //2. Check if value has changed set val is Null to reload it.
            if (val != null && !adapter.IsChanged())
                return val;

            val = await adapter.LoadAsync();

            SetToCache<TConfig>(adapter, val);

            return val;
        }

        public void Save<TConfig>(TConfig config, Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null)
            where TConfig : class
        {
            var adapter = GetAdapter(filterSelector);
            SetToCache(adapter, config);
            adapter.Save(config);
        }

        public async Task SaveAsync<TConfig>(TConfig config, Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null)
            where TConfig : class
        {
            var adapter = GetAdapter(filterSelector);
            SetToCache(adapter, config);
            await adapter.SaveAsync(config);
        }
    }
}
