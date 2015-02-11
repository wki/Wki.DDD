namespace Wki.DDD.EventBus
{
    public static class EventBusExtension
    {
        public static void Publish<T>(this IPublish obj, IEvent @event)
            where T : class, IEvent
        {
            Hub.Current.Publish(@event);
        }
    }
}
