namespace Talabat.Domain.Shared.Params
{
    public class PaginationParams
    {
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value >= 10 ? 10 : value; }
        }
        public int PageNumber { get; set; } = 1;
    }
}
