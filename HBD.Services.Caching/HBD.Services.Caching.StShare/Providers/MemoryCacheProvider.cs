#region using

using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

#endregion

namespace HBD.Services.Caching.Providers
{
    public class MemoryCacheProvider : CacheProvider
    {
        private bool _isDisposed;
        private IMemoryCache _cache;

        private IOptions<MemoryCacheOptions> Option { get; }

        public MemoryCacheProvider(TimeSpan defaultExpiration, IOptions<MemoryCacheOptions> option = null)
            : base(defaultExpiration)
        {
            Option = option ?? new MemoryCacheOptions();
            Initialize();
        }

        public MemoryCacheProvider() : this(new TimeSpan(4, 0, 0)) { }

        public override object Get(string key) => _cache.Get(key);

        protected override void SetCache(string key, object value, TimeSpan expiration)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(MemoryCacheProvider));

            _cache.Set(key, value, DateTimeOffset.Now.Add(expiration));
        }

        protected override void RemoveCache(string key) => _cache.Remove(key);

        protected override void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;
            _isDisposed = isDisposing;

            _cache.Dispose();
            _cache = null;
        }

        public override void Clear() => Initialize();

        private void Initialize()
        {
            _cache?.Dispose();
            _cache = new MemoryCache(Option);
        }
    }
}