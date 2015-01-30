namespace Wki.DDD.Domain
{
    public class AggregateRoot<IdType> : Entity<IdType>
    {
        public AggregateRoot() : base()
        {
        }

        public AggregateRoot(IdType id) : base(id)
        {
        }
    }
}
