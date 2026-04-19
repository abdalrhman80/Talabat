using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class RefundWebHookModel
    {
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = default!;

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = default!;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = default!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = default!;

        [JsonPropertyName("approvedAt")]
        public string ApprovedAt { get; set; } = default!;

    }
}
