using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class CardPaymentResponse : FawaterakBaseResponse
    {
        [JsonPropertyName("data")]
        public CardPaymentResponseDataModel Data { get; set; } = new();

        public class CardPaymentResponseDataModel : FawaterakBasePaymentResponse
        {
            [JsonPropertyName("payment_data")]
            public CardPaymentData PaymentData { get; set; } = new();

            public class CardPaymentData : FawaterakBasePaymentDataResponse
            {
                [JsonPropertyName("redirectTo")]
                public string RedirectTo { get; set; } = default!;

                public override string ToString()
                {
                    return $"RedirectTo:{RedirectTo}";
                }
            }
        }
    }
}
