namespace Talabat.Application.Products.Queries.GetProductBrands
{
    internal class GetProductBrandsQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetProductBrandsQuery, PaginationResponse<ProductBrandDto>>
    {
        public async Task<PaginationResponse<ProductBrandDto>> Handle(GetProductBrandsQuery request, CancellationToken cancellationToken)
        {
            var dbProductBrands = await _unitOfWork.Repository<ProductBrand>().GetAllWithSpecificationAsync(new ProductBrandSpecifications(request.PageSize, request.PageNumber));

            if (dbProductBrands is null || !dbProductBrands.Any())
                return new PaginationResponse<ProductBrandDto>(request.PageNumber, request.PageSize, 0, []);

            var productsBrands = _mapper.Map<IReadOnlyList<ProductBrandDto>>(dbProductBrands);

            return new PaginationResponse<ProductBrandDto>(request.PageNumber, request.PageSize, productsBrands.Count, productsBrands); ;
        }
    }
}
