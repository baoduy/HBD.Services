using System.Collections.Generic;
using HBD.Services.Transformation.Convertors;
using HBD.Services.Transformation.TokenExtractors;
using HBD.Services.Transformation.TokenResolvers;

namespace HBD.Services.Transformation
{
    public class TransformOptions
    {
        public object[] TransformData { get; set; }
        public ITokenResolver TokenResolver { get; set; }
        public IList<ITokenExtractor> TokenExtractors { get; } = new List<ITokenExtractor>();
        public IValueFormatter Formatter { get; set; }
        public bool DisabledLocalCache { get; set; }
    }
}
