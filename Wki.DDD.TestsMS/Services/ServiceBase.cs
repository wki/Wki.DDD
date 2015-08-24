using Wki.DDD.EventBus;

namespace Wki.DDD.Tests.Services
{
    public class ServiceBase : IPublish
    {
        public int nrEventsHandled;

        public ServiceBase()
        {
            nrEventsHandled = 0;
        }
    }
}
