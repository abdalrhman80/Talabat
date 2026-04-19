namespace Talabat.Domain.Models
{
    public class ProductPicture
    {
        public int Id { get; set; }
        public string PicturePath { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
