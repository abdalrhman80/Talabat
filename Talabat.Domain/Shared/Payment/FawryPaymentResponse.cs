using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class FawryPaymentResponse : FawaterakBaseResponse
    {
        [JsonPropertyName("data")]
        public FawryPaymentResponseDataModel Data { get; set; } = new();

        public class FawryPaymentResponseDataModel : FawaterakBasePaymentResponse
        {
            [JsonPropertyName("payment_data")]
            public FawryPaymentData PaymentData { get; set; } = new();

            public class FawryPaymentData : FawaterakBasePaymentDataResponse
            {
                [JsonPropertyName("fawryCode")]
                public string FawryCode { get; set; } = default!;

                [JsonPropertyName("expireDate")]
                public string ExpireDate { get; set; } = default!;

                public override string ToString()
                {
                    return $"FawryCode:{FawryCode}, ExpireDate:{ExpireDate} ";
                }
            }
        }
    }
}
