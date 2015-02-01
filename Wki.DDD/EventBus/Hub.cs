using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wki.DDD.EventBus
{
    // a hopefully readable alias...
    using EventInvokers = Dictionary<Type, IEventInvoker<IEvent>>;

    /*
     * Idea for expansion: split into several classes:
     *
     * Hub -- Publish() method, delegates to dispatcher.Dispatch()
     * IDispatcher -- dispatcher interface forces Dispatch<T>(T @event) method
     *
     */
    public class Hub : IHub
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        private readonly IContainer container;

        // Idee: Caching aller aufzurufenden Methoden pro Event.
        private EventInvokers eventInvokers;

        public static IHub Current { get; private set; }

        public Hub(IContainer container)
        {
            // maybe we should die if Current is already set.
            Current = this;
            eventInvokers = new EventInvokers();
            this.container = container;
        }

        public void Publish<T>(T @event) where T : class, IEvent
        {
            var eventType = @event.GetType();
            log.Debug(m => m("Publish: {0} Type: {1}", @event.GetType(), typeof(T)));

            if (eventInvokers.ContainsKey(eventType))
            {
                eventInvokers.Add(eventType, new EventInvoker<T>(@event));
                Dispatch<T>(@event);
                DispatchInterfaces(@event);
            }

            // TODO: call handlers
        }

        // must be public to allow reflection to find it
        // return value may be of interest to caching logic.
        public void Dispatch<T>(T @event) where T : class, IEvent
        {
            foreach (var eventHandler in container.ResolveAll<ISubscribe<T>>())
            {
                log.Debug(m => m("Handler: {0}", eventHandler));
                // eventHandler.Handle(@event);

                eventInvokers[typeof(T)].AddHandler(eventHandler.Handler);
            }
        }

        public void DispatchInterfaces(IEvent @event)
        {
            // Hint: this Query could be cached.
            foreach (var t in @event.GetType().GetInterfaces().Where(t => typeof(IEvent).IsAssignableFrom(t)))
            {
                log.Debug(m => m("Trying to Dispatch Type: {0}", t.FullName));
                MethodInfo publishMethod = this.GetType().GetMethod("Dispatch").MakeGenericMethod(t);

                publishMethod.Invoke(this, new object[] { @event } );
            }
        }
    }
}
