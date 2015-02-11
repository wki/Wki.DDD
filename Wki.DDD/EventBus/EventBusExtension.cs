namespace Wki.DDD.EventBus
{
    public static class EventBusExtension
    {
        public static void Publish<T>(this IPublish obj, T @event)
            where T : class, IEvent
        {
            Hub.Current.Publish(@event);
        }
    }
}
