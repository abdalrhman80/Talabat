namespace Talabat.Application.Wishlist.Commands.AddToWishlist
{
    internal class AddToWishlistCommandHandler(
        ILogger<AddToWishlistCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<AddToWishlistCommand>
    {
        public async Task Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var productExists = await _unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId) != null;

            if (!productExists) throw new NotFoundException("Product not found.");

            var existingItem = await _unitOfWork.Repository<WishListItem>().GetEntityWithSpecificationAsync(new WishListItemSpecifications(currentUser.Email, request.ProductId));

            if (existingItem != null) throw new BadRequestException("Product already exists in the WishList.");

            _logger.LogInformation("User {UserId} add new WishList Item {ProductId}", currentUser.Id, request.ProductId);

            var wishListItem = new WishListItem
            {
                ProductId = request.ProductId,
                UserEmail = currentUser.Email
            };

            _unitOfWork.Repository<WishListItem>().Add(wishListItem);

            await _unitOfWork.CompleteAsync();
        }
    }
}
