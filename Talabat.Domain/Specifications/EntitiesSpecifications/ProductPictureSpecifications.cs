using System.Linq.Expressions;
using Talabat.Domain.Models;
using Talabat.Domain.Shared;
using Talabat.Domain.Specifications;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class ProductPictureSpecifications : SpecificationBase<ProductPicture>
    {
        public ProductPictureSpecifications(int productId)
        {
            ApplyFilters(productId);
        }

        public ProductPictureSpecifications(int productId, int pictureId)
        {
            ApplyFilters(productId, pictureId);
        }

        private void ApplyFilters(int? productId = null, int? pictureId = null)
        {
            var expressions = new List<Expression<Func<ProductPicture, bool>>>();

            if (pictureId.HasValue) expressions.Add(p => p.Id == pictureId.Value);

            if (productId.HasValue) expressions.Add(p => p.Product.Id == productId.Value);

            if (expressions.Count > 0)
            {
                AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
            }
            else
            {
                AddCriteria(x => true);
            }
        }
    }
}
