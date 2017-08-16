using HBD.Framework;
using HBD.Framework.Attributes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.Adapters
{
    public abstract class FileConfigAdapter<TConfig> : IConfigAdapter<TConfig> where TConfig : class
    {
        private DateTime _lastLoaded = DateTime.MinValue;

        protected FileConfigAdapter([NotNull]string filePath, TimeSpan? expiration = null)
        {
            FilePath = filePath;
            Expiration = expiration;
        }

        protected FileConfigAdapter([NotNull]FileFinder fileFinder, TimeSpan? expiration = null)
        {
            FileFinder = fileFinder;
            Expiration = expiration;
        }

        private FileFinder FileFinder { get; }

        /// <summary>
        /// Default is 4 hours.
        /// </summary>
        public virtual TimeSpan? Expiration { get; }

        protected string FilePath { get; private set; }

        protected virtual void Validate()
        {
            if (FileFinder != null && FilePath.IsNullOrEmpty())
                FilePath = FileFinder.Find();

            if (!File.Exists(FilePath))
                throw new FileNotFoundException(FilePath);
        }

        public virtual bool IsChanged()
        {
            Validate();
            if (_lastLoaded == DateTime.MinValue) return true;
            var lasModify = File.GetLastWriteTime(FilePath);
            return lasModify > _lastLoaded;
        }

        public TConfig Load()
        {
            Validate();
            var text = File.ReadAllText(FilePath);
            _lastLoaded = DateTime.Now;
            return Deserialize(text);
        }

        public void Save(TConfig config)
        {
            Validate();
            var val = Serialize(config);
            File.WriteAllText(FilePath, val);
            _lastLoaded = DateTime.Now;
        }

        protected abstract TConfig Deserialize(string text);
        protected abstract string Serialize(TConfig config);

        public async Task<TConfig> LoadAsync()
        {
            Validate();

            using (var f = File.OpenText(FilePath))
            {
                var text = await f.ReadToEndAsync();
                _lastLoaded = DateTime.Now;
                return Deserialize(text);
            }
        }

        public async Task SaveAsync(TConfig config)
        {
            Validate();

            var val = Serialize(config);

            using (var writer = File.CreateText(FilePath))
                await writer.WriteAsync(val);

            _lastLoaded = DateTime.Now;
        }
    }
}
