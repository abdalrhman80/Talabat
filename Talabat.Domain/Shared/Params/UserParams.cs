using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Shared.Params
{
    public class UserParams : PaginationParams
    {
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}
