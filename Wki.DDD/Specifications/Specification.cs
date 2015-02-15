using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Specifications
{
    public class Specification<TEntity> : AbstractSpecification<TEntity>
        where TEntity: class
    {
        private Expression<Predicate<TEntity>> predicate;

        public Expression<Predicate<TEntity>> Predicate
        {
            get { return predicate; }
        }

        public Specification(Expression<Predicate<TEntity>> predicate)
        {
            this.predicate = predicate;
        }

        protected Specification(BinaryExpression expression)
            : this(Expression.Lambda<Predicate<TEntity>>(expression))
        {
        }

        public override bool IsSatisfiedBy(TEntity entity) 
        {
            return predicate.Compile().Invoke(entity);
        }
    }
}
