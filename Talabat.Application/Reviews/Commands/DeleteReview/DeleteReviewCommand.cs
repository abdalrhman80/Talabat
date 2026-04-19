namespace Talabat.Application.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(int ReviewId) : IRequest;
}
