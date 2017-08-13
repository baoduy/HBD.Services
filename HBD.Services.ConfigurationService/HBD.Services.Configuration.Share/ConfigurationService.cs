using HBD.Framework.Attributes;
using HBD.Framework.Core;
using HBD.Services.Configuration.Adapters;
using HBD.Services.Configuration.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HBD.Services.Caching.Providers;

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
        public IReadOnlyCollection<IConfigAdapter> Adapters
        {
            get
            {
                Initialize();
                return _readOnlyAdapters;
            }
        }

        public ConfigurationService()
        {
            _defaultExpiration = new TimeSpan(4, 0, 0);
            _cacheProvider = new MemoryCacheProvider(_defaultExpiration);
            _adapters = new List<IConfigAdapter>();
            _readOnlyAdapters = new ReadOnlyCollection<IConfigAdapter>(_adapters);
        }

        private void Initialize()
        {
            lock (_locker)
            {
                if (_isInitialized) return;

                //Load Adapters form Service Locator.
                if (HBD.ServiceLocator.IsServiceLocatorSet)
                    _adapters.AddRange(ServiceLocator.Current.GetAllInstances<IConfigAdapter>());

                _isInitialized = true;
            }
        }

        private IConfigAdapter<TConfig> GetAdapter<TConfig>(Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null) where TConfig : class
        {
            this.Initialize();

            var adapters = _adapters.OfType<IConfigAdapter<TConfig>>().ToList();

            if (adapters.Count > 0 && filterSelector != null)
                return filterSelector.Invoke(adapters);

            return adapters.First();
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
        }

        public virtual TConfig Get<TConfig>(
            Func<IEnumerable<IConfigAdapter<TConfig>>, IConfigAdapter<TConfig>> filterSelector = null)
            where TConfig : class
        {
            CheckDisposed();

            var cacheKey = typeof(TConfig).FullName;
            var adapter = GetAdapter<TConfig>();

            //1. Load from cache
            var val = _cacheProvider.Get<TConfig>(cacheKey);

            //2. Check if value has changed set val is Null to reload it.
            if (val != null && !adapter.IsChanged())
                return val;

            val = adapter.Load();

            var t = adapter.Expiration;
            if (t <= TimeSpan.MinValue)
                t = _defaultExpiration;

            _cacheProvider.Set(cacheKey, val, t);

            return val;
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

        public void Remove(IConfigAdapter adapter)
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

        private void Dispose(bool isDisposing)
        {
            _cacheProvider?.Dispose();
            _adapters.Clear();
            _isDisposed = isDisposing;
        }
    }
}
