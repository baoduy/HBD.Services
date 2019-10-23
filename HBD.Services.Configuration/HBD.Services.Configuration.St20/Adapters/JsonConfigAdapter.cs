using System;

namespace HBD.Services.Configuration.Adapters
{
    public class JsonConfigAdapter<TConfig> : FileConfigAdapter<TConfig> where TConfig : class
    {
        #region Constructors

        public JsonConfigAdapter( string filePath) : base(filePath)
        {
        }

        public JsonConfigAdapter( FileFinder fileFinder) : base(fileFinder)
        {
        }

        #endregion Constructors

        #region Methods

        protected override TConfig Deserialize(string text)
            => Newtonsoft.Json.JsonConvert.DeserializeObject<TConfig>(text);

        protected override string Serialize(TConfig config)
            => Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);

        protected override void Validate()
        {
            base.Validate();

            if (!FilePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Only json file is supported");
        }

        #endregion Methods
    }
}