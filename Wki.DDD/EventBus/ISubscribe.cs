namespace Wki.DDD.EventBus
{
    public interface ISubscribe<in T> where T : class, IEvent
    {
        void Handle(T @event);
    }
}
