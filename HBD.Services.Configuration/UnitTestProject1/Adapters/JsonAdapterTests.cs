using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace HBD.Services.Configuration.StTests.Adapters
{
    [TestClass]
    public class JsonAdapterTests
    {
        [TestMethod]
        public void File_Not_Exists_Exception()
        {
            var a = new JsonConfigAdapter<TestItem>("abc.json");
            Action l = () => a.Load();
            l.ShouldThrow<FileNotFoundException>();
        }

        [TestMethod]
        public void Before_Load_Is_Changes_Should_Be_True()
        {
            var a = new TestJsonConfigAdapter();
            a.IsChanged().Should().BeTrue();
        }

        [TestMethod]
        public void After_Load_Is_Changes_Should_Be_False()
        {
            var a = new TestJsonConfigAdapter();
            var t = a.Load();
            a.IsChanged().Should().BeFalse();
            t.Should().NotBeNull();
        }

        [TestMethod]
        public void After_Load_File_Changed_Is_Changes_Should_Be_True()
        {
            var a = new TestJsonConfigAdapter();
            var t = a.Load();
            a.IsChanged().Should().BeFalse();

            t.Name = "The name has been changed";
            File.WriteAllText("TestData\\json1.json", Newtonsoft.Json.JsonConvert.SerializeObject(t));

            //Wait for file is stable
            Thread.Sleep(2000);

            a.IsChanged().Should().BeTrue();
        }

        [TestMethod]
        public void Not_Supported_Exception()
        {
            var a = new JsonConfigAdapter<TestItem>("TestData\\TextFile1.txt");
            Action t=()=>a.Load();
            t.ShouldThrow<NotSupportedException>();
        }
    }
}
