using System;

namespace Wki.DDD.Domain
{
    public class ObjectIsInvalidException : Exception
    {
        public ObjectIsInvalidException(string message)
            : base(message)
        { }
    }
}
