using System;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.Services.Transformation.TokenDefinitions;
using HBD.Services.Transformation.TokenExtractors;
using HBD.Services.Transformation.TokenResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Transform.Tests.TokenResolvers
{
    [TestClass]
    public class TokenResolversTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_TokenResolver_Token_Null()
        {
            var resolver = new TokenResolver();

            var val = resolver.Resolve(null, data: new Object[]
            {
                null,
                new { A=(string)null},
                new { A=123},
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_TokenResolver_Data_Null()
        {
            var resolver = new TokenResolver();

            var val = resolver.Resolve(new TokenResult(new CurlyBracketDefinition(), "{A}", "{A} 123", 0), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_TokenResolver_Data_Empty()
        {
            var resolver = new TokenResolver();

            var val = resolver.Resolve(new TokenResult(new CurlyBracketDefinition(), "{A}", "{A} 123", 0), new object[0]);
        }

        [TestMethod]
        public void Test_TokenResolver()
        {
            var resolver = new TokenResolver();

            var val = resolver.Resolve(new TokenResult(new CurlyBracketDefinition(), "{A}", "{A} 123", 0), data: new Object[]
            {
                null,
                new { A=(string)null},
                new { A=123},
            });

            val.Should().Be(123);
        }

        [TestMethod]
        public async Task Test_TokenResolver_Async()
        {
            var resolver = new TokenResolver();

            var val =await resolver.ResolveAsync(new TokenResult(new CurlyBracketDefinition(), "{A}", "{A} 123", 0), data: new Object[]
            {
                null,
                new { A=(string)null},
                new { A=123},
            });

            val.Should().Be(123);
        }
    }
}
