using System;
using System.IO;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using HBD.Services.Polly.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polly;

namespace HBD.Services.Polly.Tests
{
    [TestClass]
    public class ClassWithVirtualMethodsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithoutPolicy()
        {
            var item = new PolicyBuilder<Item>().Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DuplicatePolicies()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2));
        }

        [TestMethod]
        public void CreateInterceptor()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .CreateInterceptor();

           Assert.IsInstanceOfType(item,typeof(IInterceptor));
        }

        [TestMethod]
        public void RetryPolicy()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build(0);

            item.Method("Duy");
            Assert.IsTrue(item.Count == 2);
        }

        [TestMethod]
        public void NonVirtualRetryPolicy()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.Method(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build(0);

            item.NonVirtualMethod();
            Assert.IsTrue(item.Count == 2);
        }

        [TestMethod]
        public async Task RetryPolicyAsync()
        {
            var item = new PolicyBuilder<Item>()
                .For(i => i.MethodAsync(null), Policy.Handle<FileNotFoundException>().Retry(2))
                .Build(0);

            var rs = await item.MethodAsync("Duy");
            Assert.IsTrue(item.Count == 2);
            Assert.IsTrue(rs == item.Count.ToString());
        }
    }
}
