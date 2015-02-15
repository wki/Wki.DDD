using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// see also: https://github.com/riteshrao/ncommon/blob/v1.2/NCommon/src/Specifications/Specification.cs

namespace Wki.DDD.Specifications
{
    public class Specification<TEntity> : ISpecification<TEntity>
        where TEntity : class
    {
        public Expression<Predicate<TEntity>> Predicate { get; set; }

        public bool IsSatisfiedBy(TEntity entity)
        {
            return Predicate.Compile().Invoke(entity);
        }

        public Specification(Expression<Predicate<TEntity>> predicate)
        {
            Predicate = predicate;
        }

        #region AND
        public static Specification<TEntity> operator &(Specification<TEntity> lhs, Specification<TEntity> rhs)
        {
            InvocationExpression rightInvoke = Expression.Invoke(
                rhs.Predicate,
                lhs.Predicate.Parameters.Cast<Expression>()
            );
            BinaryExpression newExpression = Expression.MakeBinary(
                ExpressionType.AndAlso,
                lhs.Predicate.Body,
                rightInvoke
            );
            return new Specification<TEntity>(
                Expression.Lambda<Predicate<TEntity>>(newExpression, lhs.Predicate.Parameters)
            );
        }

        public Specification<TEntity> And(Expression<Predicate<TEntity>> predicate)
        {
            return this & new Specification<TEntity>(predicate);
        }

        public Specification<TEntity> And(Specification<TEntity> other)
        {
            return this & other;
        }
        #endregion

        #region OR
        public static Specification<TEntity> operator |(Specification<TEntity> lhs, Specification<TEntity> rhs)
        {
            InvocationExpression rightInvoke = Expression.Invoke(
                rhs.Predicate,
                lhs.Predicate.Parameters.Cast<Expression>()
            );
            BinaryExpression newExpression = Expression.MakeBinary(
                ExpressionType.OrElse,
                lhs.Predicate.Body,
                rightInvoke
            );
            return new Specification<TEntity>(
                Expression.Lambda<Predicate<TEntity>>(
                    newExpression, 
                    lhs.Predicate.Parameters
                )
            );
        }

        public Specification<TEntity> Or(Expression<Predicate<TEntity>> predicate)
        {
            return this | new Specification<TEntity>(predicate);
        }

        public Specification<TEntity> Or(Specification<TEntity> other)
        {
            return this | other;
        }
        #endregion

        #region NOT
        public static Specification<TEntity> operator !(Specification<TEntity> spec)
        {
            UnaryExpression newExpression = Expression.MakeUnary(
                ExpressionType.Not,
                spec.Predicate.Body,
                typeof(Specification<TEntity>)
            );

            return new Specification<TEntity>(
                Expression.Lambda<Predicate<TEntity>>(
                    newExpression, 
                    spec.Predicate.Parameters
                )
            );
        }

        public Specification<TEntity> Not()
        {
            return !this;
        }
        #endregion
    }
}
