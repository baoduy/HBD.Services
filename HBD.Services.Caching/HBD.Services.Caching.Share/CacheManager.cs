#region using

using System;
using HBD.Services.Caching.Providers;

#endregion

namespace HBD.Services.Caching
{
    public static class CacheManager
    {
        private static ICacheProvider _currentProvider;
        private static Func<ICacheProvider> _providerLoader;

        static CacheManager()
        {
            _providerLoader = () => new MemoryCacheProvider();
        }

        public static ICacheProvider Default => GetOrLoad();

        /// <summary>
        /// Will be replace by SingletonManager.GetOrLoad in future.
        /// </summary>
        private static ICacheProvider GetOrLoad()
        {
            if (_currentProvider != null) return _currentProvider;
            _currentProvider = _providerLoader?.Invoke();
            return _currentProvider;
        }

        public static bool IsProviderSet
           => _currentProvider != null || _providerLoader != null;

        public static void SetProvider(ICacheProvider newProvider)
            => _currentProvider = newProvider ?? throw new ArgumentNullException(nameof(newProvider));

        public static void SetProvider(Func<ICacheProvider> newProviderBuilder)
        {
            _providerLoader = newProviderBuilder ?? throw new ArgumentNullException(nameof(newProviderBuilder));
            _currentProvider = null;
        }
    }
}