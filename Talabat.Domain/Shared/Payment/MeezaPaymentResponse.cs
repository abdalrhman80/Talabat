using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared.Payment
{
    public class MeezaPaymentResponse : FawaterakBaseResponse
    {
        [JsonPropertyName("data")]
        public MeezaPaymentResponseDataModel Data { get; set; } = new();

        public class MeezaPaymentResponseDataModel : FawaterakBasePaymentResponse
        {
            [JsonPropertyName("payment_data")]
            public MeezaPaymentData PaymentData { get; set; } = new();

            public class MeezaPaymentData : FawaterakBasePaymentDataResponse
            {
                [JsonPropertyName("meezaReference")]
                public string MeezaReference { get; set; } = default!;

                [JsonPropertyName("meezaQrCode")]
                public string MeezaQrCode { get; set; } = default!;

                public override string ToString()
                {
                    return $"MeezaReference:{MeezaReference}, MeezaQrCode: {MeezaQrCode}";
                }
            }
        }

    }
}
