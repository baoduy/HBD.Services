#region using

using HBD.Framework;
using HBD.Services.HtmlGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Services.HtmlGenerationTests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void ToStringTest()
        {
            var a = StyleNames.BackgroundColor.ToEnumString();
            Assert.AreEqual(a, "background-color");

            a = StyleNames.TextOverflow.ToEnumString();
            Assert.AreEqual(a, "TextOverflow");
        }

        [TestMethod]
        public void GetEnumFromTest()
        {
            var a = "background-color".ToEnum<StyleNames>();
            Assert.AreEqual(a, StyleNames.BackgroundColor);

            a = "BorderCollapse".ToEnum<StyleNames>();
            Assert.AreEqual(a, StyleNames.BorderCollapse);
        }
    }
}