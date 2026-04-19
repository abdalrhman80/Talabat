using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class FawaterakInvoiceResponseModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("data")]
        public FawaterakInvoiceResponseDataModel Data { get; set; } = new();

        public class FawaterakInvoiceResponseDataModel
        {
            [JsonPropertyName("url")]
            public string Url { get; set; } = default!;

            [JsonPropertyName("invoiceId")]
            public string InvoiceId { get; set; }= default!;

            [JsonPropertyName("invoiceKey")]
            public string InvoiceKey { get; set; }= default!;
        }
    }
}
