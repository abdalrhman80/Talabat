namespace Talabat.Application.Products.Queries.GetAllProducts
{
    internal class GetAllProductsQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetAllProductsQuery, PaginationResponse<ProductDto>>
    {
        public async Task<PaginationResponse<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productParams = new ProductParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                BrandId = request.BrandId,
                TypeId = request.TypeId,
            };

            var dbProducts = await _unitOfWork.Repository<Product>().GetAllWithSpecificationAsync(new ProductSpecifications(productParams));

            if (dbProducts is null || !dbProducts.Any())
                return new PaginationResponse<ProductDto>(request.PageNumber, request.PageSize, 0, []);

            var products = _mapper.Map<IReadOnlyList<ProductDto>>(dbProducts);

            return new PaginationResponse<ProductDto>(request.PageNumber, request.PageSize, products.Count, products); ;
        }
    }
}
