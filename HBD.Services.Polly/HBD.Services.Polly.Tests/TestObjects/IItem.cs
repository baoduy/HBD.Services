using System.Threading.Tasks;

namespace HBD.Services.Polly.Tests.TestObjects
{
    public interface IObject
    {
        void Method();
        Task MethodAsync();
    }

    public class TestItem : IObject
    {
        public int MethodCalled { get; private set; }
        public int MethodAsyncCalled { get; private set; }

        public void Method()
        {
            MethodCalled++;
        }

        public Task MethodAsync()
        {
            MethodAsyncCalled++;
            return Task.FromResult(MethodAsyncCalled);
        }
    }
}
