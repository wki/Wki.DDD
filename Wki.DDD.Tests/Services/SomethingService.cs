using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;

namespace Wki.DDD.Tests.Services
{
    public class SomethingService : ServiceBase, ISubscribe<SomethingHappened>
    {
        public void Handle(SomethingHappened @event)
        {
            nrEventsHandled++;
        }
    }
}
