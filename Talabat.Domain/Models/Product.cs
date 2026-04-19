namespace Talabat.Domain.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public string? PicturePath { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = false;
        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }

        public ProductBrand ProductBrand { get; set; } = default!;
        public ProductType ProductType { get; set; } = default!;
        public ICollection<Review> Reviews { get; set; } = default!;
        public ICollection<ProductPicture> ProductPictures { get; set; } = default!;
    }
}
