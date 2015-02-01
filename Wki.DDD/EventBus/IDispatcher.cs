namespace Wki.DDD.EventBus
{
    public interface IDispatcher
    {
        void Dispatch<T>(T @event) where T : class, IEvent;
    }
}
