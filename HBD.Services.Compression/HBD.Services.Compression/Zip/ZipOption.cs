namespace HBD.Services.Compression.Zip
{
    public abstract class ZipOption
    {
        #region Properties

        protected internal string Password { get; set; }

        protected internal IZipAdapter ZipAdapter { get; set; }

        #endregion Properties

        #region Methods

        protected virtual IZipAdapter CreateAdapter() => new ZipAdapter();

        protected IZipAdapter GetOrCreateAdapter()
                    => ZipAdapter ?? CreateAdapter();

        #endregion Methods
    }
}