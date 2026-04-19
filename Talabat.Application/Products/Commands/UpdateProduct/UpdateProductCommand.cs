namespace Talabat.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public bool? IsActive { get; set; }
        public required int ProductBrandId { get; set; }
        public required int ProductTypeId { get; set; }
    }
}
