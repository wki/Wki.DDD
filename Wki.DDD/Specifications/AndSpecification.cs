using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Specifications
{
    public class AndSpecification<TEntity> : AbstractSpecification<TEntity>
        where TEntity: class
    {
        private ISpecification<TEntity> lhs;
        private ISpecification<TEntity> rhs;

        public AndSpecification(ISpecification<TEntity> lhs, ISpecification<TEntity> rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public override bool IsSatisfiedBy(TEntity entity)
        {
            return lhs.IsSatisfiedBy(entity) && rhs.IsSatisfiedBy(entity);
        }
    }
}
