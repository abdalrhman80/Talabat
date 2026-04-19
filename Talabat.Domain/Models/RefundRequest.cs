using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Models
{
    public class RefundRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public string BuyerEmail { get; set; } = default!;
        public string Reason { get; set; } = default!;
        public RefundRequestStatus Status { get; set; } = RefundRequestStatus.Pending;
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.Now.ToLocalTime();
        public DateTimeOffset? ReviewedAt { get; set; }
        public string? AdminNotes { get; set; }
    }
}
