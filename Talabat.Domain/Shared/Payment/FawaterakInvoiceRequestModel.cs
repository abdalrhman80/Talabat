using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class FawaterakInvoiceRequestModel
    {
        [JsonPropertyName("payment_method_id")]
        public int? PaymentMethodId { get; set; }

        [JsonPropertyName("customer")]
        [Required]
        public required CustomerModel Customer { get; set; }

        [JsonPropertyName("cartItems")]
        [MinLength(1)]
        [Required]
        public required List<CartItemModel> CartItems { get; set; } = [];

        [JsonPropertyName("shipping")]
        public decimal Shipping { get; set; }

        [JsonPropertyName("cartTotal")]
        public decimal CartTotal => CartItems.Sum(item => item.Price * item.Quantity);

        [JsonPropertyName("currency")]
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "EGP";

        [JsonPropertyName("payLoad")]
        public InvoicePayload? PayLoad { get; set; } = new();

        [JsonPropertyName("redirectionUrls")]
        public RedirectionUrlsModel? RedirectionUrls { get; set; } = new();


        public class InvoicePayload
        {
            /// <summary>
            /// Your internal order ID
            /// </summary>
            public string OrderId { get; set; }
        }

        public class CustomerModel
        {
            [JsonPropertyName("unique_id")]
            public string CustomerId { get; set; } = default!;

            [JsonPropertyName("first_name")]
            [Required]
            public required string FirstName { get; set; }

            [JsonPropertyName("last_name")]
            [Required]
            public required string LastName { get; set; }

            [JsonPropertyName("email")]
            [EmailAddress]
            public string? Email { get; set; }

            [JsonPropertyName("phone")]
            [Phone]
            public string? Phone { get; set; }
        }

        public class CartItemModel
        {
            [JsonPropertyName("name")]
            [Required]
            public string Name { get; set; }

            [JsonPropertyName("price")]
            [Range(0.01, double.MaxValue)]
            public decimal Price { get; set; }

            [JsonPropertyName("quantity")]
            [Range(1, int.MaxValue)]
            public int Quantity { get; set; }
        }

        public class RedirectionUrlsModel
        {
            [JsonPropertyName("successUrl")]
            [Url]
            public string? OnSuccess { get; set; }
            [JsonPropertyName("failUrl")]
            [Url]
            public string? OnFailure { get; set; }

            [JsonPropertyName("pendingUrl")]
            [Url]
            public string? OnPending { get; set; }
        }
    }
}
