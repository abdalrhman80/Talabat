namespace Talabat.Application.Order.Queries.GetUserOrder
{
    public record GetUserOrderQuery(int OrderId) : IRequest<OrderDto>;
}
