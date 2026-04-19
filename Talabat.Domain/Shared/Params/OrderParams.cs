using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Shared.Params
{
    public class OrderParams : PaginationParams
    {
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;
        public PaymentMethodNames? PaymentMethod { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
