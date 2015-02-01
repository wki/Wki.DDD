using Common.Logging;
using System.Linq;
using System.Reflection;

namespace Wki.DDD.EventBus
{
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
        private readonly IDispatcher dispatcher;

        public static IHub Current { get; private set; }

        private Hub()
        {
            Current = this;
        }

        public Hub(IDispatcher dispatcher) : this()
        {
            this.dispatcher = dispatcher;
        }

        public Hub(IContainer container) : this()
        {
            this.dispatcher = new ImmediateDispatcher(container);
        }

        public void Publish<T>(T @event) where T : class, IEvent
        {
            log.Debug(m => m("Publish: {0} Type: {1}", @event.GetType(), typeof(T)));
            dispatcher.Dispatch(@event);
        }
    }
}
