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
    }
}
