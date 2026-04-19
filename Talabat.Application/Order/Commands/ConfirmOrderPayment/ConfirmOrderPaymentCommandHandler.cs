namespace Talabat.Application.Order.Commands.ConfirmOrderPayment
{
    internal class ConfirmOrderPaymentCommandHandler(
        ILogger<ConfirmOrderPaymentCommandHandler> _logger,
        IFawaterakPaymentService _paymentService,
        IUnitOfWork _unitOfWork,
        IOrderService _orderService
        ) : IRequestHandler<ConfirmOrderPaymentCommand>
    {
        public async Task Handle(ConfirmOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _paymentService.VerifyPaidWebhook(request);

            if (!isValid) throw new UnauthorizedAccessException("Invalid webhook signature");

            _ = int.TryParse(request.Payload?.OrderId, out int orderId);

            var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(
                new OrderSpecifications(orderId: orderId, addOrderItems: true))
                ?? throw new NotFoundException($"No order found with this id {orderId}");

            // User already cancelled
            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Refunded)
                return;

            await _orderService.UpdateOrderStatusTransactionAsync(order, OrderStatus.Success);

            _logger.LogInformation("Order {OrderId} Paid Successfully", orderId);
        }
    }
}
