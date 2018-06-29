using System.IO;
using System.Threading.Tasks;

namespace HBD.Services.Polly.Tests.TestObjects
{
    public class Item
    {
        public Item(int count)
        {
            Count = count;
        }

        public int Count { get; private set; }

        public virtual string Method(string value)
        {
            Count++;
            if (Count <= 1)
                throw new FileNotFoundException();
            return Count.ToString();
        }

        public string NonVirtualMethod()
            => Method(nameof(NonVirtualMethod));

        public virtual Task<string> MethodAsync(string value)
        {
            Count++;

            if (Count <= 1)
                throw new FileNotFoundException();

            return Task.FromResult(Count.ToString());
        }
    }

    public class Item2 : Item
    {
        public string NonVirtualMethod1()
            => Method(nameof(NonVirtualMethod));

        public Item2(int count) : base(count)
        {
        }
    }
}
