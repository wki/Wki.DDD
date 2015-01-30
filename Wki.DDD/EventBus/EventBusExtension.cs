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
