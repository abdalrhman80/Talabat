namespace Talabat.Application.Wishlist.Commands.AddToWishlist
{
    public record AddToWishlistCommand(int ProductId) : IRequest;
}
