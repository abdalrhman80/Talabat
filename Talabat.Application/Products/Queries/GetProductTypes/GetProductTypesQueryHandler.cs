namespace Talabat.Application.Products.Queries.GetProductTypes
{
    internal class GetProductTypesQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetProductTypesQuery, PaginationResponse<ProductTypeDto>>
    {
        public async Task<PaginationResponse<ProductTypeDto>> Handle(GetProductTypesQuery request, CancellationToken cancellationToken)
        {
            var dbProductTypes = await _unitOfWork.Repository<ProductType>().GetAllWithSpecificationAsync(
                new ProductTypeSpecifications(request.PageNumber, request.PageSize));

            if (dbProductTypes is null || !dbProductTypes.Any())
                return new PaginationResponse<ProductTypeDto>(request.PageNumber, request.PageSize, 0, []);

            var productTypes = _mapper.Map<IReadOnlyList<ProductTypeDto>>(dbProductTypes);

            return new PaginationResponse<ProductTypeDto>(request.PageNumber, request.PageSize, productTypes.Count, productTypes); ;
        }
    }
}
