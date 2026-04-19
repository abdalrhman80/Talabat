namespace Talabat.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, int ProductBrandId, int ProductTypeId, int StockQuantity) : IRequest<int>
    {
        public bool IsActive { get; set; } = true;
    }
}
