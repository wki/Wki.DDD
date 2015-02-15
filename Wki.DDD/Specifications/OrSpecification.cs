using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Specifications
{
    public class OrSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        private Specification<TEntity> lhs;
        private Specification<TEntity> rhs;

        public OrSpecification(Specification<TEntity> lhs, Specification<TEntity> rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public override bool IsSatisfiedBy(TEntity entity)
        {
            return lhs.IsSatisfiedBy(entity) || rhs.IsSatisfiedBy(entity);
        }
    }
}
