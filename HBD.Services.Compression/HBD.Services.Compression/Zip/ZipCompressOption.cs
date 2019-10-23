using HBD.Framework.Attributes;
using HBD.Framework.Core;
using HBD.Framework.Extensions;
using System.Collections.Generic;

namespace HBD.Services.Compression.Zip
{
    public enum ZipLevel
    {
        Storage = 0,
        Normal = 5,
        BestCompress = 9,
    }

    internal enum ZipFileOption
    {
        /// <summary>
        /// Default it will zip to the new file.
        /// The InvalidOperationException will be thrown if the file is existed.
        /// </summary>
        Default = 0,

        /// <summary>
        /// If the destination file is existed then overwrite that file.
        /// </summary>
        OverwriteIfExisted = 1,

        /// <summary>
        /// Append if the file is existed.
        /// </summary>
        AppendIfExisted = 2,
    }

    public class ZipCompressOption : ZipOption
    {
        #region Constructors

        protected internal ZipCompressOption(params string[] filesOrDirectories)
        {
            Inputs.AddRange(filesOrDirectories);
        }

        #endregion Constructors

        #region Properties

        internal ZipFileOption FileOption { get; private set; } = ZipFileOption.Default;

        protected internal IList<string> Inputs { get; } = new List<string>();

        protected internal ZipLevel Level { get; private set; } = Compression.Zip.ZipLevel.BestCompress;

        protected internal string OutputFile { get; private set; }

        #endregion Properties

        #region Methods

        public ZipCompressOption AppendIfExisted()
        {
            FileOption = ZipFileOption.AppendIfExisted;
            return this;
        }

        /// <summary>
        /// Compress and store the zip file as the same folder with the first file or folder.
        /// </summary>
        /// <returns></returns>
        public virtual string Build()
            => GetOrCreateAdapter().Compress(this);

        /// <summary>
        /// Compress to a location.
        /// </summary>
        /// <param name="outputFile"></param>
        public virtual void BuildTo([NotNull]string outputFile)
        {
            Guard.ArgumentIsNotNull(outputFile, nameof(outputFile));
            OutputFile = outputFile;
            GetOrCreateAdapter().Compress(this);
        }

        public ZipCompressOption Include([NotNull]string fileOrDirectory)
        {
            Guard.ArgumentIsNotNull(fileOrDirectory, nameof(fileOrDirectory));

            if (!Inputs.Contains(fileOrDirectory))
                Inputs.Add(fileOrDirectory);

            return this;
        }

        public ZipCompressOption OverwriteIfExisted()
        {
            FileOption = ZipFileOption.OverwriteIfExisted;
            return this;
        }

        public ZipCompressOption ZipLevel(ZipLevel level)
        {
            Level = level;
            return this;
        }

        #endregion Methods
    }
}