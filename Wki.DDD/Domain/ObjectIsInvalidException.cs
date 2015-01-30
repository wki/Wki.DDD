using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Domain
{
    public class ObjectIsInvalidException : Exception
    {
        public ObjectIsInvalidException(string message)
            : base(message)
        { }
    }
}
