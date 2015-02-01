using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.EventBus
{
    public class ImmediateDispatcher : IDispatcher
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        private readonly IContainer container;

        public ImmediateDispatcher(IContainer container)
        {
            this.container = container;
        }

        public void Dispatch<T>(T @event) where T : class, IEvent
        {
            DispatchEvent<T>(@event);
            DispatchInterfaces(@event);
        }

        // must be public to allow reflection to find it
        // return value may be of interest to caching logic.
        public void DispatchEvent<T>(T @event) where T : class, IEvent
        {
            foreach (var eventHandler in container.ResolveAll<ISubscribe<T>>())
            {
                log.Debug(m => m("Handler: {0}", eventHandler));
                eventHandler.Handle(@event);
            }
        }

        public void DispatchInterfaces(IEvent @event)
        {
            // Hint: this Query could be cached.
            foreach (var t in @event.GetType().GetInterfaces().Where(t => typeof(IEvent).IsAssignableFrom(t)))
            {
                log.Debug(m => m("Trying to Dispatch Type: {0}", t.FullName));
                MethodInfo publishMethod = this.GetType().GetMethod("DispatchEvent").MakeGenericMethod(t);

                publishMethod.Invoke(this, new object[] { @event });
            }
        }
    }
}
