using System.Collections.Generic;

namespace Wki.DDD.EventBus
{
    /// <summary>
    /// a to-be implemented resolver for your favourite IoC Container
    /// </summary>
    public interface IContainer
    {
        IEnumerable<T> ResolveAll<T>();
    }
}
