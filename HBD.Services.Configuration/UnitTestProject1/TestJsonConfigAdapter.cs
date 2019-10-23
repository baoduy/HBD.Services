using HBD.Services.Configuration.Adapters;
using System;

namespace HBD.Services.Configuration.StTests
{
    internal class TestJsonConfigAdapter : JsonConfigAdapter<TestItem>
    {
        #region Constructors

        public TestJsonConfigAdapter() : base("TestData\\json1.json")
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Caching in 1 hours.
        /// </summary>
        public override TimeSpan? Expiration => new TimeSpan(1, 0, 0);

        #endregion Properties
    }
}