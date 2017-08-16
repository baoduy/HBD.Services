using HBD.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HBD.Services.Configuration.Adapters
{
    public class XmlConfigAdapter<TConfig> : FileConfigAdapter<TConfig> where TConfig : class
    {
        public XmlConfigAdapter([NotNull] string filePath) : base(filePath)
        {
        }

        public XmlConfigAdapter([NotNull] FileFinder fileFinder) : base(fileFinder)
        {
        }

        protected override void Validate()
        {
            base.Validate();

            if (!FilePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Only XML file is supported");
        }

        protected override string Serialize(TConfig config)
        {
            var xmlserializer = new XmlSerializer(typeof(TConfig));

            using (var stringWriter = new StringWriter())
            using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                xmlserializer.Serialize(writer, config);
                return stringWriter.ToString();
            }
        }

        protected override TConfig Deserialize(string text)
        {
            var xmlserializer = new XmlSerializer(typeof(TConfig));

            using (var reader = new StringReader(text))
                return xmlserializer.Deserialize(reader) as TConfig;
        }
    }
}
