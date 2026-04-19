namespace Talabat.Application.Order.Commands.RejectRefundRequest
{
    public class RejectRefundRequestCommand : IRequest
    {
        [JsonIgnore]
        public int RequestId { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; } = default!;
    }
}
