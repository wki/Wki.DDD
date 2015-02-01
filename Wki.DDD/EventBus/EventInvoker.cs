using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.EventBus
{
    public class EventInvoker<TEvent> : IEventInvoker<TEvent>
        where TEvent: IEvent
    {
        private TEvent @event;
        private List<Action<TEvent>> handlers;

        public EventInvoker(TEvent @event)
        {
            handlers = new List<Action<TEvent>>();
            this.@event = @event;
        }

        public void AddHandler(Action<TEvent> handler)
        {
            handlers.Add(handler);
        }

        public void Invoke()
        {
            handlers.ForEach(h => h.Invoke(@event));
        }
    }
}
