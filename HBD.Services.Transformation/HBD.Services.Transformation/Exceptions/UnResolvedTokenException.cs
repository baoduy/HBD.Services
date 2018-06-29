using System;

namespace HBD.Services.Transformation.Exceptions
{
    public sealed class UnResolvedTokenException : Exception
    {
        //public UnResolvedTokenException()
        //{
        //}

        public UnResolvedTokenException(string token) : this(token,null)
        {
        }

        public UnResolvedTokenException(string token, Exception innerException) : base(token, innerException)
        {
        }
    }
}
