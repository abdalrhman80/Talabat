namespace Talabat.Application.Products.Commands.CreateProduct
{
    internal class CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<CreateProductCommand, int>
    {
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (await _unitOfWork.Repository<ProductType>().GetByIdAsync(request.ProductTypeId) is null)
                throw new NotFoundException($"No product type found with id {request.ProductTypeId}");

            if (await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(request.ProductBrandId) is null)
                throw new NotFoundException($"No product brands found with id {request.ProductBrandId}");

            _logger.LogInformation("User {UserId} is creating a new product {@Product}", currentUser.Id, request);

            var product = _mapper.Map<Product>(request);

            _unitOfWork.Repository<Product>().Add(product);

            await _unitOfWork.CompleteAsync();

            return product.Id;
        }
    }
}
