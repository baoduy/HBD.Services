using HBD.Framework.Attributes;
using System;

namespace HBD.Services.Configuration.Adapters
{
    public class JsonConfigAdapter<TConfig> : FileConfigAdapter<TConfig> where TConfig : class
    {
        public JsonConfigAdapter([NotNull] string filePath) : base(filePath)
        {
        }

        public JsonConfigAdapter([NotNull] FileFinder fileFinder) : base(fileFinder)
        {
        }

        protected override void Validate()
        {
            base.Validate();

            if (!FilePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Only json file is supported");
        }

        protected override string Serialize(TConfig config)
            => Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);

        protected override TConfig Deserialize(string text)
            => Newtonsoft.Json.JsonConvert.DeserializeObject<TConfig>(text);
    }
}
