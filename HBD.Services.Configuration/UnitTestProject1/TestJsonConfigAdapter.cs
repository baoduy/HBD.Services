using HBD.Services.Configuration.Adapters;
using System;

namespace HBD.Services.Configuration.StTests
{
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
