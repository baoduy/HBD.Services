using System.Threading.Tasks;
using HBD.Services.Transformation.TokenExtractors;

namespace HBD.Services.Transformation.TokenResolvers
{
    public interface ITokenResolver
    {
        object Resolve(IToken token, params object[]data);

        Task<object> ResolveAsync(IToken token, params object[]data);
    }
}
