using Wki.DDD.EventBus;
using System;

namespace Wki.DDD.Domain
{
    public class Entity<IdType> : DomainObject, IEquatable<Entity<IdType>>, IPublish
    {
        public IdType Id { get; private set; }

        // needed  ny EF
        protected Entity()
        { }

        public Entity(IdType id)
        {
            Id = id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public static bool operator ==(Entity<IdType> entity1, Entity<IdType> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            return entity1.Id.ToString() == entity2.Id.ToString();
        }

        public static bool operator !=(Entity<IdType> entity1, Entity<IdType> entity2)
        {
            return !(entity1 == entity2);
        }

        public bool Equals(Entity<IdType> other)
        {
            return other != null
                && other is Entity<IdType>
                && this == (Entity<IdType>)other;
        }
    }
}
