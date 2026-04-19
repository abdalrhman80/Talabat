using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Shared.Params
{
    public class ProductParams : PaginationParams
    {
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

    }
}
