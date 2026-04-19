namespace Talabat.Application.Reviews.Commands.AddReview
{
    public class AddReviewCommand : IRequest<ReviewDto>
    {
        [JsonIgnore]
        public int ProductId { get; set; }

        [Required]
        public float Rating { get; set; }

        public string? Review { get; set; } 
    }
}
