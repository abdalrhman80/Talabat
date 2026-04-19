namespace Talabat.Application.Order.Commands.RefundOrderPayment
{
    internal class RefundOrderPaymentCommandHandler(
        ILogger<RefundOrderPaymentCommandHandler> _logger,
        IUnitOfWork _unitOfWork,
        IOrderService _orderService
        ) : IRequestHandler<RefundOrderPaymentCommand>
    {
        public async Task Handle(RefundOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            _ = int.TryParse(request.TransactionId, out int invoiceId);

            await RefundOrderAsync(invoiceId, request.Reason);
        }


        private async Task RefundOrderAsync(int invoiceId, string notes)
        {
            var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(new OrderSpecifications(invoiceId.ToString(), addOrderItems: true))
                ?? throw new NotFoundException($"No order found with invoice id {invoiceId}");

            if (order.Status != OrderStatus.RefundedRequest) return;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Refunded);

                var refundRequest = await _unitOfWork.Repository<RefundRequest>().GetEntityWithSpecificationAsync(new RefundRequestSpecifications(orderId: order.Id));

                if (refundRequest != null && refundRequest.Status == RefundRequestStatus.Pending)
                {
                    refundRequest.Status = RefundRequestStatus.Completed;
                    refundRequest.AdminNotes = notes;
                    refundRequest.ReviewedAt = DateTimeOffset.Now.ToLocalTime();
                    _unitOfWork.Repository<RefundRequest>().Update(refundRequest);
                }

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Order {OrderId} payment refunded", order.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
