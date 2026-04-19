namespace Talabat.Domain.Models
{
    public class WishListItem
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.Now.ToLocalTime();
    }
}
