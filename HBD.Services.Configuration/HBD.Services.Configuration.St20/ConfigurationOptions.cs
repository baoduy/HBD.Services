using HBD.Framework.Attributes;
using HBD.Services.Configuration.Adapters;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Caching.Memory;

namespace HBD.Services.Configuration
{
    public class ConfigurationOptions
    {
        public ConfigurationOptions() => Adapters = new List<IConfigAdapter>();

        internal List<IConfigAdapter> Adapters { get; }
        internal IMemoryCache CacheProvider { get; private set; }
        internal TimeSpan? DefaultExpiration { get; private set; }
        internal bool IsIgnoreCaching { get; private set; }

        public ConfigurationOptions WithAdapters(params IConfigAdapter[] configAdapters)
        {
            foreach (var item in configAdapters)
            {
                if (!Adapters.Contains(item))
                    Adapters.Add(item);
            }
            return this;
        }

        public ConfigurationOptions WithMemoryCache(IMemoryCache cacheProvider)
        {
            CacheProvider = cacheProvider;
            return this;
        }

        /// <summary>
        /// The expiration should be from Adapters. However if any adapter is not provided the expiration the this one will be used.
        /// </summary>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public ConfigurationOptions WithExpiration([NotNull]TimeSpan expiration)
        {
            if (expiration == TimeSpan.MinValue)
                throw new ArgumentNullException(nameof(expiration));

            DefaultExpiration = expiration;
            return this;
        }

        /// <summary>
        /// Register the XML or Json file to the configuration no need a custom adapter provided.
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ConfigurationOptions RegisterFile<TConfig>(string filePath) where TConfig : class
        {
            switch (Path.GetExtension(filePath)?.ToLower())
            {
                case ".json":
                    Adapters.Add(new JsonConfigAdapter<TConfig>(filePath));
                    break;
                case ".xml":
                    Adapters.Add(new XmlConfigAdapter<TConfig>(filePath));
                    break;
                default: throw new NotSupportedException(filePath);
            }

            return this;
        }

        /// <summary>
        /// Register the XML or Json file to the configuration no need a custom adapter provided.
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="finder"></param>
        /// <returns></returns>
        public ConfigurationOptions RegisterFile<TConfig>(FileFinder finder) where TConfig : class
        {
            switch (Path.GetExtension(finder.FileName)?.ToLower())
            {
                case ".json":
                    Adapters.Add(new JsonConfigAdapter<TConfig>(finder));
                    break;
                case ".xml":
                    Adapters.Add(new XmlConfigAdapter<TConfig>(finder));
                    break;
                default: throw new NotSupportedException(finder.FileName);
            }

            return this;
        }

        /// <summary>
        /// Not using caching mechanism on the ConfigurationManager. 
        /// </summary>
        /// <returns></returns>
        public ConfigurationOptions IgnoreCaching()
        {
            IsIgnoreCaching = true;
            return this;
        }
    }
}
