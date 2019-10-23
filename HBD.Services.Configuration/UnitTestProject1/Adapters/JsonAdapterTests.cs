using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.StTests.Adapters
{
    [TestClass]
    public class JsonAdapterTests
    {
        #region Methods

        [TestMethod]
        [Ignore]
        public async Task After_Load_File_Changed_Is_Changes_Should_Be_True()
        {
            var a = new TestJsonConfigAdapter();
            var t = await a.LoadAsync();

            a.HasChanged().Should().BeFalse();

            t.Name = "The name has been changed";
            await File.WriteAllTextAsync("TestData\\json1.json", Newtonsoft.Json.JsonConvert.SerializeObject(t));

            //Wait for file is stable
            await Task.Delay(2000);

            a.HasChanged().Should().BeTrue();
        }

        [TestMethod]
        public async Task After_Load_Is_Changes_Should_Be_False()
        {
            var a = new TestJsonConfigAdapter();
            var t = await a.LoadAsync();
            a.HasChanged().Should().BeFalse();
            t.Should().NotBeNull();
        }

        [TestMethod]
        public void Before_Load_Is_Changes_Should_Be_True()
        {
            var a = new TestJsonConfigAdapter();
            a.HasChanged().Should().BeTrue();
        }

        [TestMethod]
        public void File_Not_Exists_Exception()
        {
            var a = new JsonConfigAdapter<TestItem>("abc.json");
            Action l = () => a.LoadAsync().GetAwaiter().GetResult();
            l.Should().Throw<FileNotFoundException>();
        }

        [TestMethod]
        public void Not_Supported_Exception()
        {
            var a = new JsonConfigAdapter<TestItem>("TestData\\TextFile1.txt");
            Action t = () => a.LoadAsync().GetAwaiter().GetResult();
            t.Should().Throw<NotSupportedException>();
        }

        #endregion Methods
    }
}