namespace Talabat.Application.Reviews.Queries.GetProductReview
{
    public record GetProductReviewQuery(int ProductId, int ReviewId): IRequest<ReviewDto>;
}
