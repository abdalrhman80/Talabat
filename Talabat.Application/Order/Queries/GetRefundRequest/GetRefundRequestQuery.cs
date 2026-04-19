namespace Talabat.Application.Order.Queries.GetRefundRequest
{
    public record GetRefundRequestQuery(int OrderId) : IRequest<RefundRequestDto>;
}
