namespace Talabat.Application.Order.Commands.AddRefundRequest
{
    internal class AddRefundRequestQueryHandler(
        ILogger<AddRefundRequestQueryHandler> _logger,
        IUnitOfWork _unitOfWork,
        IUserContext _userContext
        ) : IRequestHandler<AddRefundRequestQuery>
    {
        public async Task Handle(AddRefundRequestQuery request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

                _logger.LogInformation("User {UserId} make a refund request on order {orderId}", currentUser.Id, request.OrderId);

                var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(new OrderSpecifications(buyerEmail: currentUser.Email, orderId: request.OrderId))
                    ?? throw new NotFoundException($"No order found with id {request.OrderId}");

                if (order.Status != OrderStatus.Success)
                    throw new BadRequestException("You can only request a refund for a successful order.");

                var existingRequest = await _unitOfWork.Repository<RefundRequest>().GetEntityWithSpecificationAsync(new RefundRequestSpecifications(orderId: request.OrderId));

                if (existingRequest != null)
                    throw new BadRequestException($"A refund request already exists for this order with status: {existingRequest.Status}");

                var refundRequest = new RefundRequest
                {
                    OrderId = request.OrderId,
                    BuyerEmail = currentUser.Email,
                    Reason = request.Reason,
                    Status = RefundRequestStatus.Pending
                };

                order.Status = OrderStatus.RefundedRequest;

                _unitOfWork.Repository<RefundRequest>().Add(refundRequest);
                _unitOfWork.Repository<UserOrder>().Update(order);

                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
