using System.Linq.Expressions;

namespace Talabat.Domain.Shared
{
    public static class ExpressionExtensions
    {
        extension<T>(Expression<Func<T, bool>> expr1)
        {
            /// <summary>
            /// Combines two boolean expressions using AndAlso logic with proper parameter replacement
            /// </summary>
            public Expression<Func<T, bool>> And(Expression<Func<T, bool>> expr2)
            {
                var parameter = Expression.Parameter(typeof(T), "t");

                var combinedBody = Expression.AndAlso(
                    Expression.Invoke(expr1, parameter),
                    Expression.Invoke(expr2, parameter)
                );

                return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
            }

            /// <summary>
            /// Combines two boolean expressions using OrElse logic with proper parameter replacement
            /// </summary>
            public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> expr2)
            {
                var parameter = Expression.Parameter(typeof(T), "t");

                var combinedBody = Expression.OrElse(
                    Expression.Invoke(expr1, parameter),
                    Expression.Invoke(expr2, parameter)
                );

                return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
            }
        }
    }
}
