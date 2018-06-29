using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HBD.Services.Transformation.Exceptions;
using HBD.Services.Transformation.TokenDefinitions;

[assembly:InternalsVisibleTo("HBD.Services.Transform.Tests")]
namespace HBD.Services.Transformation.TokenExtractors
{
    [DebuggerDisplay("Token = {" + nameof(Token) + "}")]
    internal sealed class TokenResult : IToken
    {
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

        public ITokenDefinition Definition { get; }
        public string Token { get; }
        public int Index { get; }

        private string _key;
        public string Key
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_key)) return _key;
                _key = this.Token.Replace(Definition.Begin.ToString(), string.Empty).Replace(Definition.End.ToString(), string.Empty);
                return _key;
            }
        }

        public string OriginalString { get; }
    }
}
