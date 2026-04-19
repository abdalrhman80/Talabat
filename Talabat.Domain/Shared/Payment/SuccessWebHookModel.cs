using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class WebHookModel
    {
        [JsonPropertyName("hashKey")]
        public string HashKey { get; set; } = default!;

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; } = default!;

        [JsonPropertyName("pay_load")]
        public string? PayloadString { get; set; }

        public WebhookPayload? Payload
        {
            get
            {
                //if (field != null) return field;
                if (string.IsNullOrEmpty(PayloadString)) return null;

                try
                {
                    field = JsonSerializer.Deserialize<WebhookPayload>(PayloadString);
                }
                catch (JsonException)
                {
                    field = null;
                }
                return field;
            }

            private set;
        }

        public class WebhookPayload
        {
            [JsonPropertyName("OrderId")]
            public string? OrderId { get; set; }
        }
    }

    public class SuccessWebHookModel : WebHookModel
    {
        [JsonPropertyName("invoice_id")]
        public int InvoiceId { get; set; }

        [JsonPropertyName("invoice_key")]
        public string InvoiceKey { get; set; } = default!;

        [JsonPropertyName("invoice_status")]
        public string InvoiceStatus { get; set; } = default!;

        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; } = default!;

        [JsonPropertyName("paidAmount")]
        public int PaidAmount { get; set; }

        [JsonPropertyName("paidAt")]
        public string PaidAt { get; set; } = default!;

        [JsonPropertyName("paidCurrency")]
        public string PaidCurrency { get; set; } = default!;

        [JsonPropertyName("customerData")]
        public CustomerData Customer { get; set; } = default!;

        public class CustomerData
        {
            [JsonPropertyName("customer_unique_id")]
            public string CustomerId { get; set; } = default!;

            [JsonPropertyName("customer_first_name")]
            public string FirstName { get; set; } = default!;

            [JsonPropertyName("customer_last_name")]
            public string LastName { get; set; } = default!;

            [JsonPropertyName("customer_email")]
            public string Email { get; set; } = default!;

            [JsonPropertyName("customer_phone")]
            public string Phone { get; set; } = default!;
        }
    }

    public class CancelWebHookModel : WebHookModel
    {
        [JsonPropertyName("referenceId")]
        public string ReferenceId { get; set; } = default!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = default!;

        [JsonPropertyName("transactionKey")]
        public string TransactionKey { get; set; } = default!;
    }

    public class FailedWebHookModel
    {
        [JsonPropertyName("hashKey")]
        public string HashKey { get; set; } = default!;

        [FromForm(Name = "invoice_key")]
        public string InvoiceKey { get; set; } = default!;

        [FromForm(Name = "invoice_id")]
        public int InvoiceId { get; set; }

        [FromForm(Name = "amount")]
        public int Amount { get; set; }

        [FromForm(Name = "paidCurrency")]
        public string PaidCurrency { get; set; } = default!;

        [FromForm(Name = "errorMessage")]
        public string ErrorMessage { get; set; } = default!;

        [FromForm(Name = "response")]
        public FailedWebHookResponse? Response { get; set; }

        [FromForm(Name = "referenceNumber")]
        public string? ReferenceNumber { get; set; }

        [FromForm(Name = "payment_method")]
        public string PaymentMethod { get; set; } = default!;

        [FromForm(Name = "pay_load")]
        public string? PayloadString { get; set; }

        public WebhookPayload? Payload
        {
            get
            {
                if (string.IsNullOrEmpty(PayloadString)) return null;

                try
                {
                    field = JsonSerializer.Deserialize<WebhookPayload>(PayloadString);
                }
                catch (JsonException)
                {
                    field = null;
                }
                return field;
            }

            private set;
        }

        public class WebhookPayload
        {
            [JsonPropertyName("OrderId")]
            public string? OrderId { get; set; }
        }

        public class FailedWebHookResponse
        {
            [JsonPropertyName("gatewayCode")]
            public string GatewayCode { get; set; } = default!;

            [JsonPropertyName("gatewayRecommendation")]
            public string GatewayRecommendation { get; set; } = default!;
        }
    }
}
