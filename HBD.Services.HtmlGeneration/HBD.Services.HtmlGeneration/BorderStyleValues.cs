#region using

#endregion

using HBD.Framework.Attributes;

namespace HBD.Services.HtmlGeneration
{
    public enum BorderStyleValues
    {
        [EnumString("dotted")] Dotted,

        [EnumString("solid")] Solid,

        [EnumString("double")] Double,

        [EnumString("dashed")] Dashed
    }
}