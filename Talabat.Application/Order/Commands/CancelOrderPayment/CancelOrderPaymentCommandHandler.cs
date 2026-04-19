namespace Talabat.Application.Order.Commands.CancelOrderPayment
{
    internal class CancelOrderPaymentCommandHandler(
        ILogger<CancelOrderPaymentCommandHandler> _logger,
        IFawaterakPaymentService _paymentService,
        IUnitOfWork _unitOfWork,
        IOrderService _orderService
        ) : IRequestHandler<CancelOrderPaymentCommand>
    {
        public async Task Handle(CancelOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _paymentService.VerifyCancelWebhook(request);

            if (!isValid) throw new UnauthorizedAccessException("Invalid webhook signature");

            _ = int.TryParse(request.Payload?.OrderId, out int orderId);

            var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(new OrderSpecifications(orderId: orderId, addOrderItems: true))
                ?? throw new NotFoundException($"No order found with this id {orderId}");

            await _orderService.UpdateOrderStatusTransactionAsync(order, OrderStatus.Cancelled);

            _logger.LogInformation("Order {OrderId} payment cancelled", orderId);
        }
    }
}
