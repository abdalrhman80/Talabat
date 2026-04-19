namespace Talabat.Application.Products.Queries.GetProduct
{
    internal class GetProductQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetProductQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationAsync(new ProductSpecifications(request.Id))
                ?? throw new NotFoundException(message: $"Product with id {request.Id} not found.");

            var result = _mapper.Map<ProductDto>(product);

            return result;
        }
    }

}
