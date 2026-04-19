using Talabat.Domain.Models;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class ProductTypeSpecifications : SpecificationBase<ProductType>
    {
        public ProductTypeSpecifications(int? pageNumber = null, int? pageSize = null)
        {
            if (pageSize != null && pageNumber != null)
                ApplyPagination(pageSize.Value, (pageNumber.Value - 1) * pageSize.Value);
        }
    }
}
