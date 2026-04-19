namespace Talabat.Application.Reviews.Queries.GetProductReviews
{
    internal class GetProductReviewsQueryHandler(
        IMapper _mapper,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<GetProductReviewsQuery, IReadOnlyList<ReviewDto>>
    {
        public async Task<IReadOnlyList<ReviewDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpecificationAsync(new ReviewSpecifications(request.ProductId));

            return (reviews is null || reviews.Count == 0) ? [] : _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        }
    }

}