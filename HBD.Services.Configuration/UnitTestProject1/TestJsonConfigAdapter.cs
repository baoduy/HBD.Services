using HBD.Services.Configuration.Adapters;
using System;
using System.Composition;

namespace HBD.Services.Configuration.StTests
{
    [Export(typeof(IConfigAdapter)), Shared]
    [Export]
    class TestJsonConfigAdapter : JsonConfigAdapter<TestItem>
    {
        public TestJsonConfigAdapter() : base("TestData\\json1.json")
        {
        }

        /// <summary>
        /// Caching in 1 hours.
        /// </summary>
        public override TimeSpan? Expiration => new TimeSpan(1, 0, 0);
    }
}
