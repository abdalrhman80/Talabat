namespace Talabat.Application.Products.Commands.UpdateProduct
{
    internal class UpdateProductCommandHandler(
        ILogger<UpdateProductCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbProduct = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationAsync(new ProductSpecifications(request.Id))
                ?? throw new NotFoundException($"Product with id {request.Id} not found.");

            if (await _unitOfWork.Repository<ProductType>().GetByIdAsync(request.ProductTypeId) is null)
                throw new NotFoundException($"No product type found with id {request.ProductTypeId}");

            if (await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(request.ProductBrandId) is null)
                throw new NotFoundException($"No product brands found with id {request.ProductBrandId}");

            _logger.LogInformation("User {UserId} is updating product{ProductId} with values {@UpdatedProduct}", currentUser.Id, request.Id, request);

            dbProduct.Name = request.Name ?? dbProduct.Name;
            dbProduct.Description = request.Description ?? dbProduct.Description;
            dbProduct.Price = request.Price ?? dbProduct.Price;
            dbProduct.StockQuantity = request.StockQuantity ?? dbProduct.StockQuantity;
            dbProduct.IsActive = request.IsActive ?? dbProduct.IsActive;
            dbProduct.ProductBrandId = request.ProductBrandId;
            dbProduct.ProductTypeId = request.ProductTypeId;

            _unitOfWork.Repository<Product>().Update(dbProduct);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDto>(dbProduct);
        }
    }
}
