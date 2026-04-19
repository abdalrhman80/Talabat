using System.Linq.Expressions;
using Talabat.Domain.Models;

namespace Talabat.Domain.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; private set; } = default!;
        public List<Expression<Func<T, object>>> Includes { get; private set; } = [];
        public List<string> IncludeStrings {  get; private set; } = [];
        public Expression<Func<T, object>> OrderBy { get; private set; } = default!;
        public Expression<Func<T, object>> OrderByDescending  { get; private set; } = default!;
        public Expression<Func<T, object>> GroupBy  { get; private set; } = default!;
        public int Take { get; private set; }
        public int Skip  { get; private set; }
        public bool IsPagingEnabled  { get; private set; } = false;


        protected void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        protected void AddIncludes(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

        protected void AddIncludes(string includeExpression) => IncludeStrings.Add(includeExpression);

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;

        protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) => GroupBy = groupByExpression;

        protected void ApplyPagination(int take, int skip)
        {
            IsPagingEnabled = true;
            Take = take;
            Skip = skip;
        }
    }
}
