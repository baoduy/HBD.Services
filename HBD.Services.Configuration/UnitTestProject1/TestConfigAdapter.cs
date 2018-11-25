using System;
using System.Threading.Tasks;
using HBD.Services.Configuration.Adapters;

namespace HBD.Services.Configuration.StTests
{
    class TestConfigAdapter : IConfigAdapter<TestItem>
    {
        public int IsChangedCalled { get; private set; }
        public int LoadCalled { get; private set; }
        public TimeSpan? Expiration { get; } = new TimeSpan(0, 0, 2);

        public bool IsChanged { get; set; }

        public bool HasChanged()
        {
            IsChangedCalled++;
            return IsChanged;
        }

        public Task<TestItem> LoadAsync()
            => Task.Run(() =>
                {
                      LoadCalled++;
                      return new TestItem{Email = "drunkcoding@outlook.com", Name = "Duy"};
                });
    }
}
