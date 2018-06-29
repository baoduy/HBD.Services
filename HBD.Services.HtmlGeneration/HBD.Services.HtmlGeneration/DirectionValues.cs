#region using

#endregion

using HBD.Framework.Attributes;

namespace HBD.Services.HtmlGeneration
{
    public enum DirectionValues
    {
        [EnumString("rtl")] RightToLeft,

        [EnumString("ltr")] LeftToRight
    }
}