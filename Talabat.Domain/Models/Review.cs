namespace Talabat.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public string BuyerName { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } 
        public float Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now.ToLocalTime();
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
