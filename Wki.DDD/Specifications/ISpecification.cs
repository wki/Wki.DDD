using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wki.DDD.Specifications
{

    // https://ianhammondcooper.wordpress.com/2007/08/05/specifications-in-c-3-0/
    // https://kitchaiyong.wordpress.com/2009/10/10/repository-specification-unit-of-work-persistence-ignorance-poco-with-microsoft-entityframework-4-0-part-2/
    // https://msdn.microsoft.com/de-de/library/bb397951(v=vs.110).aspx

    public interface ISpecification<TEntity>
        where TEntity: class
    {
        Expression<Predicate<TEntity>> Predicate { get; }

        bool IsSatisfiedBy(TEntity entity);
    }
}
