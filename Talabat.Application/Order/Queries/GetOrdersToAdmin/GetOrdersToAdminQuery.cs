namespace Talabat.Application.Order.Queries.GetOrdersToAdmin
{
    public class GetOrdersToAdminQuery : PaginationParams, IRequest<PaginationResponse<OrderDto>>
    {
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;
        public PaymentMethodNames? PaymentMethod { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
