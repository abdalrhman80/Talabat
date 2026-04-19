namespace Talabat.Application.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<ReviewDto>
    {
        [JsonIgnore]
        public int ReviewId { get; set; }

        [Required]
        public float Rating { get; set; }

        public string? Review { get; set; }
    }
}
