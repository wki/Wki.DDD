using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            DispatchEvent(@event);
            DispatchInterfaces(@event);
        }

        // must be public to allow reflection to find it
        // return value may be of interest to caching logic.
        public void DispatchEvent<T>(T @event) where T : class, IEvent
        {
            log.Debug(m => m("Dispatch Event: {0}", @event));

            foreach (var eventHandler in container.ResolveAll<ISubscribe<T>>())
            {
                log.Debug(m => m("Handler: {0}", eventHandler));
                eventHandler.Handle(@event);
            }
        }

        // made public to allow a benchmark during test
        public void DispatchInterfaces(IEvent @event)
        {
            foreach (var t in ListDispatchableInterfaces(@event))
            {
                CreateDispatchMethod(t)
                    .Invoke(this, new[] { @event });
            }
        }

        #region internal helpers
        private IEnumerable<Type> ListDispatchableInterfaces(IEvent @event)
        {
            return @event
                .GetType()
                .GetInterfaces()
                .Where(t => typeof(IEvent).IsAssignableFrom(t));
        }

        private MethodInfo CreateDispatchMethod(Type t)
        {
            return this
                .GetType()
                .GetMethod("DispatchEvent")
                .MakeGenericMethod(t);
        }
        #endregion
    }
}
