using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Shared.Params
{
    public class RefundRequestParams : PaginationParams
    {
        public RefundRequestStatus? RequestStatus { get; set; }
        public bool IncludeOrderItems { get; set; } = false;
    }
}
