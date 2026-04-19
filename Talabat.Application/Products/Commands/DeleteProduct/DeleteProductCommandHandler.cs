namespace Talabat.Application.Products.Commands.DeleteProduct
{
    internal class DeleteProductCommandHandler(
        ILogger<DeleteProductCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id)
                ?? throw new NotFoundException($"Product with ID {request.Id} not found.");

            _logger.LogInformation("User {UserId} is deleting product {ProductId}", currentUser.Id, request.Id);

            _unitOfWork.Repository<Product>().Delete(dbProduct);
            await _unitOfWork.CompleteAsync();
        }
    }
}
