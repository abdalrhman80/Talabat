namespace Talabat.Application.Order.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
