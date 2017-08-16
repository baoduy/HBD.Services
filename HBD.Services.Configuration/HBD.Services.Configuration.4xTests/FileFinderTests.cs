using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HBD.Services.Configuration.StTests
{
    [TestClass]
    public class FileFinderTests
    {
        [TestMethod]
        public void FindFile_DirectoryNotFound()
        {
            Action f = () => new FileFinder().Find("duy.json").In("AAA").Find();

            f.ShouldThrow<DirectoryNotFoundException>();
        }

        [TestMethod]
        public void FindFile_FileNotFound()
        {
            Action f = () => new FileFinder().Find("duy.json").In("TestData").Find();

            f.ShouldThrow<FileNotFoundException>();
        }

        [TestMethod]
        public void FindFile_WithFolder()
        {
            var f = new FileFinder().Find("json1.json").In("TestData").Find();

            f.Should().NotBeNullOrEmpty();
            File.Exists(f).Should().BeTrue();
        }

        [TestMethod]
        public void FindFile_WithoutFolder()
        {
            var f = new FileFinder().Find("json1.json").Find();

            f.Should().NotBeNullOrEmpty();
            File.Exists(f).Should().BeTrue();
        }
    }
}
