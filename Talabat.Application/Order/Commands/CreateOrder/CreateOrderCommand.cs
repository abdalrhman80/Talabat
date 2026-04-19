namespace Talabat.Application.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderWithInvoiceDto>
    {
        public required string BasketId { get; set; }
        public int PaymentMethodId { get; set; }
        public int DeliveryMethodId { get; set; }
        public required OrderDto.ShippingAddressDto ShippingAddress { get; set; }
    }
}
