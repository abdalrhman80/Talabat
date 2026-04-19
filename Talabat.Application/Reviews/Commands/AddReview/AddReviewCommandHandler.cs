namespace Talabat.Application.Reviews.Commands.AddReview
{
    internal class AddReviewCommandHandler(
        ILogger<AddReviewCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<AddReviewCommand, ReviewDto>
    {
        public async Task<ReviewDto> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var orderItemsPurchased = await _unitOfWork.Repository<UserOrder>().GetAllWithSpecificationAsync(new OrderSpecifications(currentUser.Email, productId: request.ProductId));

            if (orderItemsPurchased is null || !orderItemsPurchased.Any(o => o.Status == OrderStatus.Success))
                throw new BadRequestException("You haven't purchased this product yet.");

            var existingReview = await _unitOfWork.Repository<Review>().GetEntityWithSpecificationAsync(new ReviewSpecifications(currentUser.Email, productId: request.ProductId));

            if (existingReview != null)
                throw new BadRequestException("You have already reviewed this product.");


            _logger.LogInformation("User {UserId} add review on his purchased product {review}", currentUser.Id, new
            {
                BuyerEmail = currentUser.Email,
                BuyerName = currentUser.UserName!,
                request.ProductId,
                request.Rating,
                Comment = request.Review,
            });

            var newReview = new Review
            {
                BuyerEmail = currentUser.Email,
                BuyerName = currentUser.UserName!,
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Review,
            };

            _unitOfWork.Repository<Review>().Add(newReview);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ReviewDto>(newReview);
        }
    }
}
