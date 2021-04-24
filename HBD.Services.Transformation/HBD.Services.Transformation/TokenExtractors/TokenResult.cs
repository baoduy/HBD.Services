using HBD.Services.Transformation.Exceptions;
using HBD.Services.Transformation.TokenDefinitions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HBD.Services.Transform.Tests")]

namespace HBD.Services.Transformation.TokenExtractors
{
    [DebuggerDisplay("Token = {" + nameof(Token) + "}")]
    internal sealed class TokenResult : IToken
    {
        #region Fields

        private string _key;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The Token result
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="token"></param>
        /// <param name="originalString"></param>
        /// <param name="index"></param>
        public TokenResult(ITokenDefinition definition, string token, string originalString, int index)
        {
            OriginalString = originalString ?? throw new ArgumentNullException(nameof(originalString));
            Index = index;
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Token = token ?? throw new ArgumentNullException(nameof(token));

            if (!Definition.IsToken(Token))
                throw new InvalidTokenException(token);

            if (index < 0 || index > originalString.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
        }

        #endregion Constructors

        #region Properties

        public ITokenDefinition Definition { get; }

        public int Index { get; }

        public string Key
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_key)) return _key;
                _key = Token.Replace(Definition.Begin.ToString(), string.Empty).Replace(Definition.End.ToString(), string.Empty);
                return _key;
            }
        }

        public string OriginalString { get; }

        public string Token { get; }

        #endregion Properties
    }
}