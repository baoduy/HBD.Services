using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Configuration.StTests.Adapters
{
    [TestClass]
    public class XmlConfigAdapterTests
    {
        [TestMethod]
        public void LoadXML()
        {
            var a = new XmlConfigAdapter<TestItem>(new FileFinder().Find("XMLFile1.xml"));
            var t = a.Load();

            t.Should().NotBeNull();
            t.Name.Should().Be("Hoang Duy");
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        [TestMethod]
        public void SaveXML()
        {
            var a = new XmlConfigAdapter<TestItem>(new FileFinder().Find("XMLFile1.xml"));
            var t = a.Load();

            t.Email = "abc@123.com";

            a.Save(t);

            var t1 = a.Load();
            t1.Should().NotBeNull();
            t1.Name.Should().Be("Hoang Duy");
            t1.Email.Should().Be("abc@123.com");
        }
    }
}
