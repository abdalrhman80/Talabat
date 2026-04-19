namespace Talabat.Application.Order.Queries.GetRefundRequestToAdmin
{
    public record GetRefundRequestToAdminQuery(int RequestId) : IRequest<RefundRequestDto>;
}
