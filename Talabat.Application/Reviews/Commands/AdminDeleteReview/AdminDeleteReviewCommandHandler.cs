namespace Talabat.Application.Reviews.Commands.AdminDeleteReview
{
    internal class AdminDeleteReviewCommandHandler(
        ILogger<AdminDeleteReviewCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<AdminDeleteReviewCommand>
    {
        public async Task Handle(AdminDeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbReview = await _unitOfWork.Repository<Review>().GetByIdAsync(request.ReviewId) ?? throw new NotFoundException("No review found.");

            _logger.LogInformation("Admin {UserId} delete review {ReviewId} on product {ProductId}", currentUser.Id, dbReview.Id, dbReview.ProductId);

            _unitOfWork.Repository<Review>().Delete(dbReview);
            await _unitOfWork.CompleteAsync();
        }
    }
}
