using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;

namespace Wki.DDD.Tests.Services
{
    public class SomethingErrorService : ServiceBase, ISubscribe<SomethingHappened>, ISubscribe<ErrorOccured>
    {
        public void Handle(SomethingHappened @event)
        {
            nrEventsHandled++;
        }

        public void Handle(ErrorOccured @event)
        {
            nrEventsHandled += 100;
        }
    }
}
