namespace Talabat.Application.Reviews.Queries.GetProductReviews
{
    public record GetProductReviewsQuery(int ProductId) : IRequest<IReadOnlyList<ReviewDto>>;
}
