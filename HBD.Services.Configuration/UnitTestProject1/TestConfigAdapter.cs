using HBD.Services.Configuration.Adapters;
using System;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.StTests
{
    internal class TestConfigAdapter : IConfigAdapter<TestItem>
    {
        #region Properties

        public TimeSpan? Expiration { get; } = new TimeSpan(0, 0, 2);

        public bool IsChanged { get; set; }

        public int IsChangedCalled { get; private set; }

        public int LoadCalled { get; private set; }

        #endregion Properties

        #region Methods

        public bool HasChanged()
        {
            IsChangedCalled++;
            return IsChanged;
        }

        public Task<TestItem> LoadAsync()
            => Task.Run(() =>
                {
                    LoadCalled++;
                    return new TestItem { Email = "drunkcoding@outlook.com", Name = "Duy" };
                });

        #endregion Methods
    }
}