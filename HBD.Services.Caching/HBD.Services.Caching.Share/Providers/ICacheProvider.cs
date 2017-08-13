#region using

using System;
using System.Collections.Generic;

#endregion

namespace HBD.Services.Caching.Providers
{
    public interface ICacheProvider : IDisposable
    {
        bool Contains(string key);

        object Get(string key);

        T Get<T>(string key);

        void Set(string key, object value);

        void Set(string key, object value, TimeSpan expiration);

        object Remove(string key);

        IReadOnlyCollection<string> Keys { get; }
        void Clear();
    }
}