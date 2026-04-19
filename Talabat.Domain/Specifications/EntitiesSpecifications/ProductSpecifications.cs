using System.Linq.Expressions;
using Talabat.Domain.Models;
using Talabat.Domain.Shared;
using Talabat.Domain.Shared.Constants;
using Talabat.Domain.Shared.Params;
using Talabat.Domain.Specifications;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class ProductSpecifications : SpecificationBase<Product>
    {
        public ProductSpecifications(int id)
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductPictures);

            ApplyFilters(id: id);
        }

        public ProductSpecifications(ProductParams productParams)
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductPictures);

            ApplyPagination(productParams.PageSize, (productParams.PageNumber - 1) * productParams.PageSize);
            ApplySorting(productParams.SortBy, productParams.SortDirection);
            ApplyFilters(brandId: productParams.BrandId, typeId: productParams.TypeId);
        }


        #region Helper Methods

        private void ApplySorting(string? sortBy, SortDirection sortDirection)
        {
            var key = sortBy?.Trim().ToLowerInvariant();

            var sortSelectors = new Dictionary<string, Expression<Func<Product, object>>>
            {
                { nameof(Product.Id).ToLower(), p => p.Id },
                { nameof(Product.Name).ToLower(), p => p.Name! },
                { nameof(Product.Price).ToLower(), p => p.Price },
            };

            if (string.IsNullOrEmpty(key) || !sortSelectors.ContainsKey(key))
            {
                key = nameof(Product.Id).ToLower();
            }

            var selector = sortSelectors[key];

            if (sortDirection == SortDirection.Descending)
            {
                ApplyOrderByDescending(selector);
            }
            else
            {
                ApplyOrderBy(selector);
            }
        }

        private void ApplyFilters(int? id = null, int? brandId = null, int? typeId = null)
        {
            var expressions = new List<Expression<Func<Product, bool>>>();

            if (brandId.HasValue) expressions.Add(p => p.ProductBrand.Id == brandId.Value);

            if (typeId.HasValue) expressions.Add(p => p.ProductType.Id == typeId.Value);

            if (id.HasValue) expressions.Add(p => p.Id == id.Value);

            expressions.Add(p => p.IsActive);

            if (expressions.Count > 0)
            {
                AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
            }
            else
            {
                AddCriteria(x => true);
            }
        }

        #endregion
    }
}
