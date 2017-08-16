using System;
using System.Threading.Tasks;
using HBD.Services.Configuration.Adapters;
using HBD.Services.Configuration.StTests;
using System.Composition;

namespace HBD.Services.Configuration.StTests
{
    [Export(typeof(IConfigAdapter)), Shared]
    [Export]
    class TestConfigAdapter : IConfigAdapter<TestItem>
    {
        public int IsChangedCalled { get; private set; }
        public int LoadCalled { get; private set; }
        public int SaveCalled { get; private set; }
        public TimeSpan? Expiration { get; } = new TimeSpan(0, 0, 2);

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

        public Task<TestItem> LoadAsync()
            => Task.Run<TestItem>(() =>
                {
                      LoadCalled++;
                      return new TestItem();
                });

        public void Save(TestItem config)
        {
            SaveCalled++;
        }

        public Task SaveAsync(TestItem config) => Task.Run(() => SaveCalled++);

        internal void ResetCount()
        {
            SaveCalled = 0;
            IsChangedCalled = 0;
        }
    }
}
