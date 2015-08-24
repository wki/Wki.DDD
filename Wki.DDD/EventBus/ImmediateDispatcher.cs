using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wki.DDD.EventBus
{
    public class ImmediateDispatcher : IDispatcher
    {
        private static ILog log = LogManager.GetLogger<ImmediateDispatcher>();
        // TODO: concurrent dictionary ???
        private static Dictionary<Type, List<MethodInfo>> methodCache = new Dictionary<Type, List<MethodInfo>>();
        private readonly IContainer container;

        public ImmediateDispatcher(IContainer container)
        {
            this.container = container;
        }

        public void Dispatch<T>(T @event) where T : class, IEvent
        {
            DispatchEvent(@event);
            DispatchBaseTypes(@event);
        }

        // must be public to allow reflection to find it
        // return value may be of interest to caching logic.
        public bool DispatchEvent<T>(T @event) where T : class, IEvent
        {
            var handlersCalled = false;
            log.Debug(m => m("Dispatch Event: {0}", @event));

            foreach (var eventHandler in container.ResolveAll<ISubscribe<T>>())
            {
                log.Debug(m => m("Handler: {0}", eventHandler));
                eventHandler.Handle(@event);
                handlersCalled = true;
            }

            return handlersCalled;
        }

        // made public to allow a benchmark during test
        public void DispatchBaseTypes(IEvent @event)
        {
            // caching speeds up by approx. factor 7 for very simple things
            var eventType = @event.GetType();
            if (methodCache.ContainsKey(eventType))
            {
                methodCache[eventType].ToList()
                    .ForEach(m => m.Invoke(this, new[] { @event }));
            }
            else
            {

                var handledDispatchMethods = new List<MethodInfo>();

                foreach (var t in @event.GetAllBaseTypes())
                {
                    MethodInfo dispatchMethod = CreateDispatchMethod(t);
                    object returnValue = dispatchMethod.Invoke(this, new[] { @event });

                    if ((bool)returnValue)
                        handledDispatchMethods.Add(dispatchMethod);
                }

                methodCache.Add(eventType, handledDispatchMethods);
            }
        }

        private MethodInfo CreateDispatchMethod(Type t)
        {
            return this
                .GetType()
                .GetMethod("DispatchEvent")
                .MakeGenericMethod(t);
        }
    }
}
