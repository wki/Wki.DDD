using Wki.DDD.Domain;
using Wki.DDD.EventBus;
namespace Wki.DDD.Tests.Services
{
    public class CatchAllService : ServiceBase, ISubscribe<IEvent>
    {
        public void Handle(IEvent @event)
        {
            nrEventsHandled++;
        }
    }
}
