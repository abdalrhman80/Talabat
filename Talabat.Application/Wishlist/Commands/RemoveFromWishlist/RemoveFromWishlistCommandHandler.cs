namespace Talabat.Application.Wishlist.Commands.RemoveFromWishlist
{
    internal class RemoveFromWishlistCommandHandler(
        ILogger<RemoveFromWishlistCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<RemoveFromWishlistCommand>
    {
        public async Task Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var existingItem = await _unitOfWork.Repository<WishListItem>().GetEntityWithSpecificationAsync(new WishListItemSpecifications(currentUser.Email, request.ProductId))
                ?? throw new NotFoundException("Wishlist doesn't contain this product.");

            _logger.LogInformation("User {UserId} remove Item {ProductId} from his WishList", currentUser.Id, existingItem.ProductId);

            _unitOfWork.Repository<WishListItem>().Delete(existingItem);

            await _unitOfWork.CompleteAsync();
        }
    }
}
