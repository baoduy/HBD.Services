using System;
using System.IO;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.Adapters
{
    public abstract class FileConfigAdapter<TConfig> : IConfigAdapter<TConfig> where TConfig : class
    {
        #region Fields

        private DateTime _lastLoaded = DateTime.MinValue;

        #endregion Fields

        #region Constructors

        protected FileConfigAdapter(string filePath, TimeSpan? expiration = null)
        {
            FilePath = filePath;
            Expiration = expiration;
        }

        protected FileConfigAdapter(FileFinder fileFinder, TimeSpan? expiration = null)
        {
            FileFinder = fileFinder;
            Expiration = expiration;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc />
        public virtual TimeSpan? Expiration { get; }

        protected string FilePath { get; private set; }

        private FileFinder FileFinder { get; }

        #endregion Properties

        #region Methods

        public virtual bool HasChanged()
        {
            Validate();
            if (_lastLoaded == DateTime.MinValue) return true;
            var lasModify = File.GetLastWriteTime(FilePath);
            return lasModify > _lastLoaded;
        }

        public async Task<TConfig> LoadAsync()
        {
            Validate();

            using (var f = File.OpenText(FilePath))
            {
                var text = await f.ReadToEndAsync().ConfigureAwait(false);
                _lastLoaded = DateTime.Now;
                return Deserialize(text);
            }
        }

        protected abstract TConfig Deserialize(string text);

        //public void Save(TConfig config)
        //{
        //    Validate();
        //    var val = Serialize(config);
        //    File.WriteAllText(FilePath, val);
        //    _lastLoaded = DateTime.Now;
        //}
        protected abstract string Serialize(TConfig config);

        protected virtual void Validate()
        {
            if (FileFinder != null && string.IsNullOrEmpty(FilePath))
                FilePath = FileFinder.Find();

            if (!File.Exists(FilePath))
                throw new FileNotFoundException(FilePath);
        }

        #endregion Methods

        //public TConfig Load()
        //{
        //    Validate();
        //    var text = File.ReadAllText(FilePath);
        //    _lastLoaded = DateTime.Now;
        //    return Deserialize(text);
        //}
        //public async Task SaveAsync(TConfig config)
        //{
        //    Validate();

        //    var val = Serialize(config);

        //    using (var writer = File.CreateText(FilePath))
        //        await writer.WriteAsync(val).ConfigureAwait(false);

        //    _lastLoaded = DateTime.Now;
        //}
    }
}