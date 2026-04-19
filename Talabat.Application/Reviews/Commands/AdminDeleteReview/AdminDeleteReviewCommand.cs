namespace Talabat.Application.Reviews.Commands.AdminDeleteReview
{
    public record AdminDeleteReviewCommand(int ReviewId) : IRequest;
}
