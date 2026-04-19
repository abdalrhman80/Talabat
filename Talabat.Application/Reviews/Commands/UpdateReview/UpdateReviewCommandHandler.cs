namespace Talabat.Application.Reviews.Commands.UpdateReview
{
    internal class UpdateReviewCommandHandler(
        ILogger<UpdateReviewCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<UpdateReviewCommand, ReviewDto>
    {
        public async Task<ReviewDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbReview = await _unitOfWork.Repository<Review>().GetEntityWithSpecificationAsync(new ReviewSpecifications(currentUser.Email, reviewId: request.ReviewId))
                ?? throw new NotFoundException("No review found.");

            _logger.LogInformation("User {UserId} update his review on product {productId}", currentUser.Id, dbReview.ProductId);

            dbReview.Rating = request.Rating;
            dbReview.Comment = request.Review ?? dbReview.Comment;
            dbReview.UpdatedAt = DateTime.UtcNow.ToLocalTime();

            _unitOfWork.Repository<Review>().Update(dbReview);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ReviewDto>(dbReview);
        }
    }
}
