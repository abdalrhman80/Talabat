namespace Talabat.Application.Wishlist.Queries.GetWishlist
{
    internal class GetWishlistQueryHandler(
        ILogger<GetWishlistQueryHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetWishlistQuery, PaginationResponse<WishListDto>>
    {
        public async Task<PaginationResponse<WishListDto>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} get his own WishList", currentUser.Id);

            var wishListItems = await _unitOfWork.Repository<WishListItem>().GetAllWithSpecificationAsync(
                new WishListItemSpecifications(currentUser.Email, request.PageNumber, request.PageSize)
                );

            var paginationResponse = new PaginationResponse<WishListDto>(
                request.PageNumber,
                request.PageSize, 
                wishListItems.Count, 
                _mapper.Map<IReadOnlyList<WishListDto>>(wishListItems) ?? []
                );

            return paginationResponse;
        }
    }
}
