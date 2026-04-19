using System.Runtime.Serialization;

namespace Talabat.Domain.Shared.Constants
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 1,

        [EnumMember(Value = "Success")]
        Success,

        [EnumMember(Value = "Failed")]
        Failed,

        [EnumMember(Value = "Cancelled")]
        Cancelled,

        [EnumMember(Value = "RefundedRequest")]
        RefundedRequest,

        [EnumMember(Value = "Refunded")]
        Refunded
    }
}
