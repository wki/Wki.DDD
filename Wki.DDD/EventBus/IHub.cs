namespace Wki.DDD.EventBus
{
    public interface IHub
    {
        void Publish<T>(T @event) where T : class, IEvent;
    }
}
