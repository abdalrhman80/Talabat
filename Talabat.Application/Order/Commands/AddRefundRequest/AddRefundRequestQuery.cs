namespace Talabat.Application.Order.Commands.AddRefundRequest
{
    public class AddRefundRequestQuery : IRequest
    {
        [JsonIgnore]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Reason { get; set; } = default!;
    }
}
