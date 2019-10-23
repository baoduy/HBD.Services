﻿using FluentAssertions;
using HBD.Services.Transformation;
using HBD.Services.Transformation.TokenExtractors;
using HBD.Services.Transformation.TokenResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.Services.Transform.Tests
{
    [TestClass]
    public class TransformTests
    {
        #region Methods

        [TestMethod]
        public void Transform_CustomContructor_Test()
        {
            var t = new TransformerService(op =>
            {
                op.TokenExtractors.Add(new CurlyBracketExtractor());
                op.TokenResolver = new TokenResolver();
                op.DisabledLocalCache = true;
            });

            t.Tokens.Count.Should().BeGreaterOrEqualTo(1);
            t.TokenResolver.Should().NotBeNull();
            t.Formatter.Should().NotBeNull();
            t.DisabledLocalCache.Should().BeTrue();
        }

        [TestMethod]
        public void Transform_DefaultContructor_Test()
        {
            var t = new TransformerService();

            t.Tokens.Count.Should().BeGreaterOrEqualTo(3);
            t.TokenResolver.Should().NotBeNull();
            t.Formatter.Should().NotBeNull();
            t.DisabledLocalCache.Should().BeFalse();
        }

        //[TestMethod]
        //public void Transform_Test()
        //{
        //    using (var t = new Transformer(op => op.DisabledLocalCache = true))
        //    {
        //        var s = t.Transform("Hoang [A] Duy", new { A = "Bao" });
        //        s.Should().Be("Hoang Bao Duy");
        //    }
        //}

        [TestMethod]
        public async Task Transform_HugeTemplate_Async__DisableCache_Test()
        {
            var d = Path.GetDirectoryName(typeof(TransformTests).Assembly.Location);
            var template = File.ReadAllText(d + "\\TestData\\Data.txt");

            using (var t = new TransformerService(op => op.DisabledLocalCache = true))
            {
                var s = await t.TransformAsync(template, new { A = "Hoang", B = "Bao", C = "Duy", D = "HBD" });

                s.Should().ContainAll("Hoang", "Bao", "Duy", "HBD")
                    .And.Subject.Should().NotContainAny("{", "[", "<");
            }
        }

        [TestMethod]
        public async Task Transform_HugeTemplate_Async_DataProvider_Test()
        {
            var d = Path.GetDirectoryName(typeof(TransformTests).Assembly.Location);
            var template = File.ReadAllText(d + "\\TestData\\Data.txt");

            using (var t = new TransformerService())
            {
                var s = await t.TransformAsync(template, token => Task.FromResult("Duy" as object));

                s.Should().Contain("Duy")
                    .And.Subject.Should().NotContainAny("{", "[", "<");
            }
        }

        [TestMethod]
        public async Task Transform_HugeTemplate_Async_Test()
        {
            var d = Path.GetDirectoryName(typeof(TransformTests).Assembly.Location);
            var template = File.ReadAllText(d + "\\TestData\\Data.txt");

            using (var t = new TransformerService())
            {
                var s = await t.TransformAsync(template, new { A = "Hoang", B = "Bao", C = "Duy", D = "HBD" });

                s.Should().ContainAll("Hoang", "Bao", "Duy", "HBD")
                    .And.Subject.Should().NotContainAny("{", "[", "<");
            }
        }

        [TestMethod]
        public async Task TransformAsync_DefaultData_Test()
        {
            using (var t = new TransformerService(op => op.TransformData = new object[] { new { A = "Bao" } }))
            {
                t.TransformData.Any().Should().BeTrue();

                var s = await t.TransformAsync("Hoang [A] Duy");
                s.Should().Be("Hoang Bao Duy");
            }
        }

        [TestMethod]
        public async Task TransformAsync_Test()
        {
            using (var t = new TransformerService())
            {
                var s = await t.TransformAsync("Hoang [A] Duy", new { A = "Bao" });
                s.Should().Be("Hoang Bao Duy");
            }
        }

        #endregion Methods

        //[TestMethod]
        //[ExpectedException(typeof(UnResolvedTokenException))]
        //public void Transform_UnResolvedTokenException_Test()
        //{
        //    using (var t = new Transformer())
        //    {
        //        var s = t.Transform("Hoang [A] Duy", new { B = "Bao" });
        //    }
        //}

        //[TestMethod]
        //public void Transform_PrivateProperty_Test()
        //{
        //    using (var t = new Transformer())
        //    {
        //        var s = t.Transform("My name is [Name], and Birthday is <Birthday>", new TestItem());

        //        s.Should().Contain("Duy").And.Subject.Should().Contain(DateTime.Now.Year.ToString());
        //    }
        //}

        //[TestMethod]
        //public void Transform_HugeTemplate_Test()
        //{
        //    var d = Path.GetDirectoryName(typeof(TransformTests).Assembly.Location);
        //    var template = File.ReadAllText(d + "\\TestData\\Data.txt");

        //    using (var t = new Transformer())
        //    {
        //        var s = t.Transform(template, new { A = "Hoang", B = "Bao", C = "Duy", D = "HBD" });

        //        s.Should().ContainAll("Hoang", "Bao", "Duy", "HBD")
        //            .And.Subject.Should().NotContainAny("{", "[", "<");
        //    }
        //}
        //[TestMethod]
        //public void Transform_HugeTemplate_DataProvider_Test()
        //{
        //    var d = Path.GetDirectoryName(typeof(TransformTests).Assembly.Location);
        //    var template = File.ReadAllText(d + "\\TestData\\Data.txt");

        //    using (var t = new Transformer())
        //    {
        //        var s = t.Transform(template, token => new { A = "Hoang", B = "Bao", C = "Duy", D = "HBD" });

        //        s.Should().ContainAll("Hoang", "Bao", "Duy", "HBD")
        //            .And.Subject.Should().NotContainAny("{", "[", "<");
        //    }
        //}
    }
}