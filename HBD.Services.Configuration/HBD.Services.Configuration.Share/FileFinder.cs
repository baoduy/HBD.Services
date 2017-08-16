using HBD.Framework;
using HBD.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HBD.Services.Configuration
{
    /// <summary>
    /// Find a particular file in a special folder.
    /// If folder is not provided then it will find in the application folder.
    /// </summary>
    public class FileFinder
    {
        /// <summary>
        /// Find the find folder.
        /// </summary>
        /// <returns></returns>
        protected internal virtual string Find()
        {
            if (_inDirectory.IsNullOrEmpty())
            {
#if NETSTANDARD1_6
                 _inDirectory = AppContext.BaseDirectory;
#else
                _inDirectory = AppDomain.CurrentDomain.BaseDirectory;
#endif
            }

            if (!Directory.Exists(_inDirectory))
                throw new DirectoryNotFoundException(_inDirectory);

            var file = Directory.GetFiles(_inDirectory, FileName, SearchOption.AllDirectories).FirstOrDefault();

            if (file == null)
                throw new FileNotFoundException(FileName);

            return file;
        }

        internal string FileName { get; private set; }
        private string _inDirectory;

        /// <summary>
        /// File name only no need to provide the full location.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileFinder Find([NotNull]string fileName)
        {
            FileName = fileName;
            return this;
        }

        public FileFinder In([NotNull]string directory)
        {
            _inDirectory = directory;
            return this;
        }
    }
}
