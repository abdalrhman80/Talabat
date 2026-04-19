namespace Talabat.Application.Reviews.Queries.GetProductReview
{
    internal class GetProductReviewQueryHandler(
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetProductReviewQuery, ReviewDto>
    {
        public async Task<ReviewDto> Handle(GetProductReviewQuery request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Repository<Review>().GetEntityWithSpecificationAsync(new ReviewSpecifications(request.ProductId, request.ReviewId))
                ?? throw new NotFoundException("No review found for this product.");

            return _mapper.Map<ReviewDto>(review);
        }
    }
}
