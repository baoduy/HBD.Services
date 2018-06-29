using FluentAssertions;
using HBD.Services.Transformation.TokenDefinitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Transform.Tests.TokenDefinitions
{
    [TestClass]
    public class BracesTokenDefinitionTests
    {
        [TestMethod]
        public void BracesTokenDefinitionTest()
        {
            var t = new SquareBracketDefinition();

            t.IsToken("[Duy]")
                .Should().BeTrue();

            t.IsToken("{Duy}")
                .Should().BeFalse();

            t.IsToken("<Duy")
                .Should().BeFalse();

            t.IsToken("Duy>")
                .Should().BeFalse();
        }
    }
}
