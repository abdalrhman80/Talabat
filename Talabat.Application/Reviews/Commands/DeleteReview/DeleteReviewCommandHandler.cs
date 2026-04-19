namespace Talabat.Application.Reviews.Commands.DeleteReview
{
    internal class DeleteReviewCommandHandler(
        ILogger<DeleteReviewCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<DeleteReviewCommand>
    {
        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbReview = await _unitOfWork.Repository<Review>().GetEntityWithSpecificationAsync(new ReviewSpecifications(currentUser.Email, reviewId: request.ReviewId))
             ?? throw new NotFoundException("No review found.");

            _logger.LogInformation("User {UserId} delete his own review on product {ProductId}", currentUser.Id, dbReview.ProductId);

            _unitOfWork.Repository<Review>().Delete(dbReview);
            await _unitOfWork.CompleteAsync();
        }
    }
}
