using System.Runtime.Serialization;

namespace Talabat.Domain.Shared.Constants
{
    public enum RefundRequestStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 1,

        [EnumMember(Value = "Completed")]
        Completed,

        [EnumMember(Value = "Rejected")]
        Rejected
    }
}
