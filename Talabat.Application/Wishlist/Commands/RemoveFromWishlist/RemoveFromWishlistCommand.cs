namespace Talabat.Application.Wishlist.Commands.RemoveFromWishlist
{
    public record RemoveFromWishlistCommand(int ProductId) : IRequest;
}
