using HBD.Framework.Attributes;
using HBD.Framework.Core;
using System.Linq;

namespace HBD.Services.Compression.Zip
{
    public class ZipExtractOption : ZipOption
    {
        #region Constructors

        internal ZipExtractOption(string zipFile) => ZipFile = zipFile;

        #endregion Constructors

        #region Properties

        internal bool IsOverwriteIfExisted { get; private set; } = false;

        protected internal string OutputFolder { get; private set; }

        protected internal string ZipFile { get; }

        #endregion Properties

        #region Methods

        public ZipExtractOption OverwriteFilesIfExisted()
        {
            IsOverwriteIfExisted = true;
            return this;
        }

        /// <summary>
        /// Extract to percifict folder.
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        public string[] To([NotNull]string outputFolder)
        {
            Guard.ArgumentIsNotNull(outputFolder, nameof(outputFolder));
            OutputFolder = outputFolder;

            return GetOrCreateAdapter().Extract(this).ToArray();
        }

        /// <summary>
        /// Extect to the same folder of zip file.
        /// </summary>
        /// <returns></returns>
        public string[] ToTheSameFolder()
        {
            return GetOrCreateAdapter().Extract(this).ToArray();
        }

        #endregion Methods
    }
}