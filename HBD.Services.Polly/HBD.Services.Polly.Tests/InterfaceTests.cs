using System;
using System.IO;
using System.Threading.Tasks;
using HBD.Services.Polly.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polly;

namespace HBD.Services.Polly.Tests
{
    [TestClass]
    public class InterfaceTests
    {
        [TestMethod]
        public void RetryPolicy()
        {
            var item = new PolicyBuilder<IObject>()
                .For(i => i.Method(), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build<TestItem>();

            item.Method();
            Assert.IsTrue(((TestItem)item).MethodCalled > 0);
        }

        [TestMethod]
        public async Task RetryPolicyAsync()
        {
            var item = new PolicyBuilder<IObject>()
                .For(i => i.MethodAsync(), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build<TestItem>();

            await item.MethodAsync();
            Assert.IsTrue(((TestItem)item).MethodAsyncCalled > 0);
        }
    }
}
