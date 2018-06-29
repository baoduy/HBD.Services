using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HBD.Services.Transformation.TokenExtractors;

namespace HBD.Services.Transformation.TokenResolvers
{
    public class TokenResolver : ITokenResolver
    {
        protected virtual PropertyInfo GetProperty(object data, string propertyName)
        {
            var val = data.GetType().GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);

            if (val == null)
                val = data.GetType().GetProperty(propertyName,
                   BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.NonPublic);

            return val;
        }


        //=> data.GetType().GetProperties().FirstOrDefault(p =>
        //    p.CanRead && p.Name.EndsWith(propertyName, StringComparison.CurrentCultureIgnoreCase));

        /// <summary>
        /// Get the first not null value of the publict property of data.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">if data or token is null</exception>
        public virtual object Resolve(IToken token, params object[] data)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (data == null || data.Length <= 0)
                throw new ArgumentNullException(nameof(data));

            var propertyName = token.Key;

            return (from d in data
                    where d != null
                    let p = GetProperty(d, propertyName)
                    where p != null
                    select p.GetValue(d) into val
                    where val != null
                    select val).FirstOrDefault();
        }

        public Task<object> ResolveAsync(IToken token, params object[] data)
            => Task.Run(() => this.Resolve(token, data));
    }
}
