using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{ 
    public class FawaterakPaymentMethodResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("data")]
        public List<FawaterakPaymentMethodResponseData> Data { get; set; } = [];

        public class FawaterakPaymentMethodResponseData
        {
            ///// <summary>
            ///// Internal payment method ID
            ///// </summary>
            //public int Id { get; set; }

            [JsonPropertyName("paymentId")]
            public int PaymentId { get; set; }

            [JsonPropertyName("name_en")]
            public string NameEn { get; set; }

            [JsonPropertyName("name_ar")]
            public string NameAr { get; set; }

            [JsonPropertyName("redirect")]
            public string Redirect { get; set; }

            [JsonPropertyName("logo")]
            public string Logo { get; set; }
        }
    }
}
