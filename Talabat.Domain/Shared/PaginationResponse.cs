namespace Talabat.Domain.Shared
{
    public class PaginationResponse<T>(int pageNumber, int pageSize, int totalCount, IReadOnlyList<T> data) where T : class
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = totalCount;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}
