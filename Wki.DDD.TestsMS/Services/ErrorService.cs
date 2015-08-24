using Wki.DDD.EventBus;
using Wki.DDD.Tests.Events;

namespace Wki.DDD.Tests.Services
{
    public class ErrorService : ServiceBase, ISubscribe<ErrorOccured>
    {
        public void Handle(ErrorOccured @event)
        {
            nrEventsHandled += 100;
        }
    }
}
