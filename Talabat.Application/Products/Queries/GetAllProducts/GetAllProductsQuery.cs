namespace Talabat.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : PaginationParams, IRequest<PaginationResponse<ProductDto>>
    {
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
    }
}
