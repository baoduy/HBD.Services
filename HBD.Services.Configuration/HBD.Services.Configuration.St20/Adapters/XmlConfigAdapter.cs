using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HBD.Services.Configuration.Adapters
{
    public class XmlConfigAdapter<TConfig> : FileConfigAdapter<TConfig> where TConfig : class
    {
        #region Constructors

        public XmlConfigAdapter( string filePath) : base(filePath)
        {
        }

        public XmlConfigAdapter( FileFinder fileFinder) : base(fileFinder)
        {
        }

        #endregion Constructors

        #region Methods

        protected override TConfig Deserialize(string text)
        {
            var xmlserializer = new XmlSerializer(typeof(TConfig));

            using (var reader = new StringReader(text))
                return xmlserializer.Deserialize(reader) as TConfig;
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

        protected override void Validate()
        {
            base.Validate();

            if (!FilePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Only XML file is supported");
        }

        #endregion Methods
    }
}