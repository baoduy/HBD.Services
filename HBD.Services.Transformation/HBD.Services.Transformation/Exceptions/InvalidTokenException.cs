using System;

namespace HBD.Services.Transformation.Exceptions
{
    public sealed class InvalidTokenException : Exception
    {
        //public InvalidTokenException()
        //{
        //}

        public InvalidTokenException(string token) : this(token, null)
        {
        }

        public InvalidTokenException(string token, Exception innerException) : base(token, innerException)
        {
        }
    }
}
