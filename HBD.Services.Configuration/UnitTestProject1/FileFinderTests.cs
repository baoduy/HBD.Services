using System.Threading.Tasks;
using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Configuration.StTests
{
    [TestClass]
    public class FileFinderTests
    {
        [TestMethod]
        public async Task FileFinder_Test()
        {
            var config = new JsonConfigAdapter<TestItem>(new FileFinder().Find("json1.json"));
           (await config.LoadAsync()).Should().NotBeNull();
        }
    }
}
