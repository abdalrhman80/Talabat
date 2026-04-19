using Talabat.Domain.Models;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class ProductBrandSpecifications : SpecificationBase<ProductBrand>
    {
        public ProductBrandSpecifications(int? pageSize = null, int? pageNumber = null)
        {
            if (pageSize != null && pageNumber != null)
                ApplyPagination(pageSize.Value, (pageNumber.Value - 1) * pageSize.Value);
        }
    }
}
