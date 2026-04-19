using Microsoft.EntityFrameworkCore;

namespace Talabat.Domain.Specifications
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> dbSet, ISpecification<T> specification)
        {
            var query = dbSet;

            if (specification.Criteria != null)
                query = query.Where(specification.Criteria);

            if (specification.Includes != null)
                query = specification.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            if (specification.IncludeStrings != null && specification.IncludeStrings.Count > 0)
                query = specification.IncludeStrings.Aggregate(query, (currentQuery, includeString) => currentQuery.Include(includeString));

            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy).AsQueryable();

            if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending).AsQueryable();

            if (specification.GroupBy != null)
                query = query.GroupBy(specification.GroupBy).SelectMany(g => g);

            if (specification.IsPagingEnabled)
                query = query.Skip(specification.Skip).Take(specification.Take);

            return query;
        }
    }
}
