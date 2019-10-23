using FluentAssertions;
using HBD.Services.Transformation.TokenDefinitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Transform.Tests.TokenDefinitions
{
    [TestClass]
    public class BracketsTokenDefinitionTests
    {
        #region Methods

        [TestMethod]
        public void BracketsTokenDefinitionTest()
        {
            var t = new CurlyBracketDefinition();

            t.IsToken("{Duy}")
                .Should().BeTrue();

            t.IsToken("[Duy]")
                .Should().BeFalse();

            t.IsToken("<Duy")
                .Should().BeFalse();

            t.IsToken("Duy>")
                .Should().BeFalse();
        }

        #endregion Methods
    }
}