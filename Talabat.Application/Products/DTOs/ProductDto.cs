namespace Talabat.Application.Products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IList<ProductPictureDto>? Pictures { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ProductBrandId { get; set; }
        public required string ProductBrand { get; set; }
        public int ProductTypeId { get; set; }
        public required string ProductType { get; set; }
        public bool IsActive { get; set; }


        public class ProductPictureDto
        {
            public int Id { get; set; }
            public required string PictureUrl { get; set; }
        }
    }
}
