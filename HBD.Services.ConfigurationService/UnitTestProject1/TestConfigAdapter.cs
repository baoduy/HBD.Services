using HBD.Services.Configuration.Adapters;
using System;
using System.Composition;

namespace HBD.Services.Configuration.StTests
{
    [Export(typeof(IConfigAdapter)),Shared]
    [Export]
    class TestConfigAdapter : IConfigAdapter<TestItem>
    {
        public int IsChangedCalled { get; private set; }
        public int LoadCalled { get; private set; }
        public TimeSpan Expiration { get; } = new TimeSpan(0, 0, 2);

        public bool HasChanged { get; set; }

        public bool IsChanged()
        {
            IsChangedCalled++;
            return HasChanged;
        }

        public TestItem Load()
        {
            LoadCalled++;
            return new TestItem();
        }
    }
}
