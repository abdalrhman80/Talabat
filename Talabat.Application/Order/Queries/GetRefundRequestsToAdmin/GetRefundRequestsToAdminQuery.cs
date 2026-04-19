namespace Talabat.Application.Order.Queries.GetRefundRequestsToAdmin
{
    public class GetRefundRequestsToAdminQuery : PaginationParams, IRequest<PaginationResponse<RefundRequestDto>>
    {
        public RefundRequestStatus? RequestStatus { get; set; }
        public bool IncludeOrderItems { get; set; } = false;
    }
}
