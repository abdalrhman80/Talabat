using System.Runtime.Serialization;

namespace Talabat.Domain.Shared.Constants
{
    public enum PaymentMethodNames
    {
        [EnumMember(Value = "Card")]
        Card = 1,
        [EnumMember(Value = "Fawry")]
        Fawry,
        [EnumMember(Value = "Wallet")]
        EWallet
    }
}
