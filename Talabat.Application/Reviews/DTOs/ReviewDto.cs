namespace Talabat.Application.Reviews.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = default!;
        public float Rating { get; set; }
        public string? Review { get; set; }
        public string CreatedAt { get; set; } = default!;
        public string? UpdatedAt { get; set; }
    }
}
