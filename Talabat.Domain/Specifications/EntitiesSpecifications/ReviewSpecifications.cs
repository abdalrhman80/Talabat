using System.Linq.Expressions;
using Talabat.Domain.Models;
using Talabat.Domain.Shared;
using Talabat.Domain.Specifications;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class ReviewSpecifications : SpecificationBase<Review>
    {
        public ReviewSpecifications(int productId)
        {
            AddCriteria(r => r.ProductId == productId);
        }

        public ReviewSpecifications(int productId, int reviewId)
        {
            var expressions = new List<Expression<Func<Review, bool>>>()
            {
                r => r.ProductId == productId,
                r => r.Id == reviewId
            };

            AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
        }

        public ReviewSpecifications(string buyerEmail, int? productId = null, int? reviewId = null)
        {
            var expressions = new List<Expression<Func<Review, bool>>>()
            {
                r => r.BuyerEmail == buyerEmail,
                r => !productId.HasValue || r.ProductId == productId.Value,
                r => !reviewId.HasValue || r.Id == reviewId.Value
            };

            AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
        }
    }
}
