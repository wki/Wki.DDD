using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Specifications
{
    public abstract class AbstractSpecification<TEntity> : ISpecification<TEntity>
        where TEntity: class
    {
        protected AbstractSpecification()
        {
        }

        public virtual bool IsSatisfiedBy(TEntity entity)
        {
            return false;
        }

        
        // usage: specification.And(lambda)
        public ISpecification<TEntity> And(Expression<Predicate<TEntity>> otherPredicate)
        {
            return And(new Specification<TEntity>(otherPredicate));
        }

        // usage: specification.And(new spec)
        // returns: new specification
        public ISpecification<TEntity> And(ISpecification<TEntity> other)
        {
            return new AndSpecification<TEntity>(this, other);
        }

        public ISpecification<TEntity> Or(Expression<Predicate<TEntity>> otherPredicate)
        {
            return Or(new Specification<TEntity>(otherPredicate));
        }

        public ISpecification<TEntity> Or(ISpecification<TEntity> other)
        {
            return new OrSpecification<TEntity>(this, other);
        }

    }
}
