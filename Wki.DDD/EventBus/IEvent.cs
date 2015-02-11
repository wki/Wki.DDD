using System;
using System.Collections.Generic;
namespace Wki.DDD.EventBus
{
    public interface IEvent
    {
        // do not return our type, only interfaces and base class types
        IEnumerable<Type> GetAllBaseTypes();
    }
}
