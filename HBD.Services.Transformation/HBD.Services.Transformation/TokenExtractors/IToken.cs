using HBD.Services.Transformation.TokenDefinitions;

namespace HBD.Services.Transformation.TokenExtractors
{
    public interface IToken
    {
        #region Properties

        /// <summary>
        /// The token defination
        /// </summary>
        ITokenDefinition Definition { get; }

        /// <summary>
        /// The start index of token in the original string.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The key only of token.
        /// </summary>
        string Key { get; }

        string OriginalString { get; }

        /// <summary>
        /// The token value. Ex: [key]
        /// </summary>
        string Token { get; }

        #endregion Properties
    }
}