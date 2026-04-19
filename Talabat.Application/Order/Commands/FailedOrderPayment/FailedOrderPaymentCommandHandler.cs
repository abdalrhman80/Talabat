namespace Talabat.Application.Order.Commands.FailedOrderPayment
{
    internal class FailedOrderPaymentCommandHandler(
        ILogger<FailedOrderPaymentCommandHandler> _logger,
        IFawaterakPaymentService _paymentService,
        IUnitOfWork _unitOfWork,
        IOrderService _orderService
        ) : IRequestHandler<FailedOrderPaymentCommand>
    {
        public async Task Handle(FailedOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _paymentService.VerifyFailedWebhook(request);

            if (!isValid) throw new UnauthorizedAccessException("Invalid webhook signature");

            _ = int.TryParse(request.Payload?.OrderId, out int orderId);

            var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(new OrderSpecifications(orderId: orderId, addOrderItems: true))
                ?? throw new NotFoundException($"No order found with this id {orderId}");

            await _orderService.UpdateOrderStatusTransactionAsync(order, OrderStatus.Failed);

            _logger.LogInformation("Order {OrderId} Payment Failed", orderId);
        }
    }
}
