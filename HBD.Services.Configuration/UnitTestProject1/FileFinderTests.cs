using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.StTests
{
    [TestClass]
    public class FileFinderTests
    {
        #region Methods

        [TestMethod]
        public async Task FileFinder_Test()
        {
            var config = new JsonConfigAdapter<TestItem>(new FileFinder().Find("json1.json"));
            (await config.LoadAsync()).Should().NotBeNull();
        }

        #endregion Methods
    }
}