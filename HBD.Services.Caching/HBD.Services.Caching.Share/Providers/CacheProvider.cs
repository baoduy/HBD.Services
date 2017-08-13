using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HBD.Services.Caching.Providers
{
    public abstract class CacheProvider : ICacheProvider
    {
        public TimeSpan DefaultExpiration { get; }
        private readonly IList<string> _keys;

        protected CacheProvider(TimeSpan defaultExpiration)
        {
            DefaultExpiration = defaultExpiration;
            _keys = new List<string>();
            Keys = new ReadOnlyCollection<string>(_keys);
        }

        public IReadOnlyCollection<string> Keys { get; }
        public abstract void Clear();

        public bool Contains(string key) => _keys.Contains(key);

        public void Dispose() => this.Dispose(true);

        protected virtual void Dispose(bool isDisposing) { }

        public abstract object Get(string key);

        public T Get<T>(string key)
        {
            try
            {
                var value = Get(key);
                if (value==null) return default(T);
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }

        public object Remove(string key)
        {
            var val = Get(key);
            RemoveCache(key);
            _keys.Remove(key);
            return val;
        }

        protected abstract void RemoveCache(string key);

        protected abstract void SetCache(string key, object value, TimeSpan expiration);

        public void Set(string key, object value) => Set(key, value, DefaultExpiration);

        public void Set(string key, object value, TimeSpan expiration)
        {
            SetCache(key, value, expiration);
            if (!_keys.Contains(key))
                _keys.Add(key);
        }
    }
}
