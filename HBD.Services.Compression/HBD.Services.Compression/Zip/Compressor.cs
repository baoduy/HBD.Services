﻿using HBD.Framework.Attributes;
using HBD.Framework.Core;

namespace HBD.Services.Compression.Zip
{
    public static class Compressor
    {
        #region Methods

        public static ZipCompressOption Compress(params string[] files)
            => new ZipCompressOption(files);

        public static ZipExtractOption Extract(string zipFile)
           => new ZipExtractOption(zipFile);

        public static TOption WithAdapter<TOption>(this TOption @this, [NotNull]IZipAdapter adapter)
            where TOption : ZipOption
        {
            Guard.ArgumentIsNotNull(adapter, nameof(adapter));
            @this.ZipAdapter = adapter;

            return @this;
        }

        public static TOption WithPassword<TOption>(this TOption @this, [NotNull]string password)
             where TOption : ZipOption
        {
            Guard.ArgumentIsNotNull(password, nameof(password));
            @this.Password = password;

            return @this;
        }

        #endregion Methods
    }
}