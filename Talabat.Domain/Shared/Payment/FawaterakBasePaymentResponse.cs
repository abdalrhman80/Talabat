using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class FawaterakBaseResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;
    }

    public class FawaterakBasePaymentResponse
    {
        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("invoice_key")]
        public string InvoiceKey { get; set; } = default!;

        //[JsonPropertyName("payment_data")]
        //public object PaymentData { get; set; } = default!;
    }

    public class FawaterakBasePaymentDataResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
