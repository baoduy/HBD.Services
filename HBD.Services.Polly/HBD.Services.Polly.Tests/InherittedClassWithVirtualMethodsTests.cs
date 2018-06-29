using System;
using System.IO;
using System.Threading.Tasks;
using HBD.Services.Polly.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polly;

namespace HBD.Services.Polly.Tests
{
    [TestClass]
    public class InherittedClassWithVirtualMethodsTests
    {
        [TestMethod]
        public void RetryPolicy()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build<Item2>(0);

            item.Method("Duy");
            Assert.IsTrue(item.Count == 2);
            Assert.IsInstanceOfType(item,typeof(Item2));
        }

        [TestMethod]
        public void NonVirtualRetryPolicy()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build<Item2>(0);

            item.NonVirtualMethod();
            Assert.IsTrue(item.Count == 2);
            Assert.IsInstanceOfType(item,typeof(Item2));
        }

        [TestMethod]
        public void NonVirtualRetryPolicy2()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build<Item2>(0);

            ((Item2)item).NonVirtualMethod1();
            Assert.IsTrue(item.Count == 2);
        }

        [TestMethod]
        public async Task RetryPolicyAsync()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.MethodAsync(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build<Item2>(0);

            var rs = await item.MethodAsync("Duy");
            Assert.IsTrue(item.Count == 2);
            Assert.IsTrue(rs == item.Count.ToString());
            Assert.IsInstanceOfType(item,typeof(Item2));
        }
    }
}
