using System;
using Wki.DDD.EventBus;

namespace Wki.DDD.Domain
{
    public class DomainEvent : IEvent
    {
        public DateTime OccuredOn { get; private set; }

        public DomainEvent()
        {
            OccuredOn = DateTime.Now;
        }

        // do not return our type, only interfaces and base class types
        public System.Collections.Generic.IEnumerable<Type> GetAllBaseTypes()
        {
            var myType = this.GetType();
            foreach (Type implementedInterface in myType.GetInterfaces()) {
                if (typeof(IEvent).IsAssignableFrom(implementedInterface))
                    yield return implementedInterface;
            }

            var type = myType.BaseType;
            while (type != null 
                && typeof(IEvent).IsAssignableFrom(type))
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
