using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Specifications
{
    public class Specification<TEntity> : ISpecification<TEntity>
        where TEntity: class
    {
        private Expression<Predicate<TEntity>> predicate;

        public Expression<Predicate<TEntity>> Predicate
        {
            get { return predicate; }
        }

        // TODO: factor out into AbstractSpecification
        protected Specification()
        {
        }

        public Specification(Expression<Predicate<TEntity>> predicate)
        {
            this.predicate = predicate;
        }

        protected Specification(BinaryExpression expression)
            : this(Expression.Lambda<Predicate<TEntity>>(expression))
        {
        }

        public virtual bool IsSatisfiedBy(TEntity entity) 
        {
            return predicate.Compile().Invoke(entity);
        }

        #region operators
        // usage: specification.And(lambda)
        public ISpecification<TEntity> And(Expression<Predicate<TEntity>> otherPredicate)
        {
            return new AndSpecification<TEntity>(
                this,
                new Specification<TEntity>(otherPredicate)
            );
        }

        // usage: specification.And(new spec)
        // returns: new specification
        public ISpecification<TEntity> And(ISpecification<TEntity> other)
        {
            return And(other.Predicate);
        }

        public ISpecification<TEntity> Or(Expression<Predicate<TEntity>> otherPredicate)
        {
            return
                new OrSpecification<TEntity>(
                    this,
                    new Specification<TEntity>(otherPredicate)
                );
        }

        public ISpecification<TEntity> Or(ISpecification<TEntity> other)
        {
            return Or(other.Predicate);
        }

        #endregion
    }
}
