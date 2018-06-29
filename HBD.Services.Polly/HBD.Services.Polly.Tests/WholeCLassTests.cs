using System;
using System.IO;
using System.Threading.Tasks;
using HBD.Services.Polly.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polly;

namespace HBD.Services.Polly.Tests
{
    [TestClass]
    public class WholeCLassTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DuplicatePolicies()
        {
            var item = new PolicyBuilder<Item>()
                .ForAllOthers(Policy.Handle<FileNotFoundException>().Retry(2))
                .ForAllOthers(Policy.Handle<FileNotFoundException>().Retry(2));
        }

        [TestMethod]
        public async Task RetryPolicyAsync()
        {
            var item = new PolicyBuilder<Item>()
                .ForAllOthers(Policy.Handle<FileNotFoundException>().Retry(2))
                .Build(0);

            await item.MethodAsync("Duy");
            item.Method("Duy");

            Assert.IsTrue(item.Count >= 3);
        }
    }
}
