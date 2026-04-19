using System.Linq.Expressions;
using Talabat.Domain.Models;
using Talabat.Domain.Shared;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class WishListItemSpecifications : SpecificationBase<WishListItem>
    {
        public WishListItemSpecifications(string userEmail, int pageNumber, int pageSize)
        {
            AddCriteria(w => w.UserEmail == userEmail);

            ApplyPagination(pageSize, (pageNumber - 1) * pageSize);

            AddIncludes(w => w.Product);

            AddIncludes($"{nameof(Product)}.{nameof(Product.ProductPictures)}");
            AddIncludes($"{nameof(Product)}.{nameof(Product.ProductBrand)}");
            AddIncludes($"{nameof(Product)}.{nameof(Product.ProductType)}");
        }

        public WishListItemSpecifications(string userEmail, int productId)
        {
            var expressions = new List<Expression<Func<WishListItem, bool>>>()
            {
                w => w.UserEmail == userEmail,
                w => w.ProductId == productId,
            };

            if (expressions.Count > 0) AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
        }
    }
}
