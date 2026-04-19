using System.Text.Json;
using System.Text.Json.Serialization;
using Talabat.Domain.Shared;

namespace Talabat.Domain.Shared.Payment
{
    public class FawaterakInvoiceResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("data")]
        public FawaterakInvoiceData Data { get; set; } = default!;
    }

    public class FawaterakInvoiceData
    {
        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("invoice_key")]
        public string InvoiceKey { get; set; } = default!;

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; } = default!;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = default!;

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("total_paid")]
        public string TotalPaid { get; set; } = default!;

        [JsonPropertyName("status_text")]
        public string StatusText { get; set; } = default!;

        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; } = default!;

        [JsonPropertyName("paid_at")]
        public string? PaidAt { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = default!;

        [JsonPropertyName("invoice_transactions")]
        [JsonConverter(typeof(FlexibleListConverter<FawaterakInvoiceTransaction>))]
        public List<FawaterakInvoiceTransaction> InvoiceTransactions { get; set; } = [];
    }

    public class FawaterakInvoiceTransaction
    {
        [JsonPropertyName("transactionId")]
        public int TransactionId { get; set; }

        [JsonPropertyName("refrence_id")]
        public string? ReferenceId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = default!;

        [JsonPropertyName("paid_at")]
        public string? PaidAt { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; } = default!;

        [JsonPropertyName("transactionAmount")]
        public JsonElement TransactionAmount { get; set; }

        [JsonPropertyName("transactionCurrency")]
        public string? TransactionCurrency { get; set; }
    }
}
