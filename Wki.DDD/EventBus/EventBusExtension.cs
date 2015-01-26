using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.EventBus
{
    public static class EventBusExtension
    {
        public static void Publish<T>(this IPublish obj, IEvent @event)
        {
            Hub.Current.Publish(@event);
        }
    }
}
