using System;
using System.IO;
using System.Linq;

namespace HBD.Services.Configuration
{
    /// <summary>
    /// Find a particular file in a special folder.
    /// If folder is not provided then it will find in the application folder.
    /// </summary>
    public class FileFinder
    {
        #region Fields

        private string _inDirectory;

        #endregion Fields

        #region Properties

        internal string FileName { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// File name only no need to provide the full location.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileFinder Find(string fileName)
        {
            FileName = fileName;
            return this;
        }

        public FileFinder In(string directory)
        {
            _inDirectory = directory;
            return this;
        }

        /// <summary>
        /// Find the find folder.
        /// </summary>
        /// <returns></returns>
        protected internal virtual string Find()
        {
            if (string.IsNullOrEmpty(_inDirectory))
            {
                _inDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (!Directory.Exists(_inDirectory))
                throw new DirectoryNotFoundException(_inDirectory);

            var file = Directory.GetFiles(_inDirectory, FileName, SearchOption.AllDirectories).FirstOrDefault();

            if (file == null)
                throw new FileNotFoundException(FileName);

            return file;
        }

        #endregion Methods
    }
}