using System;
using System.Threading.Tasks;

namespace HBD.Services.Transformation
{
    public interface ITransformer : IDisposable
    {
        /// <summary>
        /// The TransformData that sharing for all transforming.
        /// </summary>
        object[]TransformData { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="additionalData">the additional Data that is not in the sharing data. the value in additionalData will overwrite the value from TransformData as well</param>
        /// <returns></returns>
        string Transform(string template, params object[] additionalData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="additionalData">the additional Data that is not in the sharing data. the value in additionalData will overwrite the value from TransformData as well</param>
        /// <returns></returns>
        Task<string> TransformAsync(string template, params object[] additionalData);
    }
}
