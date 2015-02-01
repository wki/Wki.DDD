using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wki.DDD.EventBus
{
    public interface IEventInvoker<out TEvent> where TEvent: IEvent
    {
        void AddHandler(Action<TEvent> handler);
        void Invoke();
    }
}
