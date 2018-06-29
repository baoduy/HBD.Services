#region using

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace HBD.Services.HtmlGeneration
{
    public class StyleCollection : Dictionary<string, string>
    {
        public StyleCollection()
        {
        }

        public StyleCollection(int capacity) : base(capacity)
        {
        }

        public StyleCollection(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public StyleCollection(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        public StyleCollection(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public StyleCollection(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        protected StyleCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string this[StyleNames name] => this[name.ToStyleName()];

        public void Add(StyleNames name, string value)
            => Add(name.ToStyleName(), value);

        public bool Remove(StyleNames name)
            => Remove(name.ToStyleName());

        public bool TryGetValue(StyleNames name, out string value)
            => base.TryGetValue(name.ToStyleName(), out value);

        public virtual bool ContainsKey(StyleNames name) => ContainsKey(name.ToStyleName());

        public override string ToString()
        {
            var builder = new StringBuilder();
            Generate(builder);
            return builder.ToString();
        }

        public virtual void Generate(StringBuilder builder)
        {
            foreach (var v in this)
                builder.AppendFormat("{0}:{1};", v.Key, v.Value);
        }
    }
}