namespace Talabat.Application.Order.Commands.UpdateOrderStatus
{
    internal class UpdateOrderStatusCommandHandler(
        ILogger<UpdateOrderStatusCommandHandler> _logger,
        IUnitOfWork _unitOfWork,
        IUserContext _userContext,
        IOrderService _orderService
        ) : IRequestHandler<UpdateOrderStatusCommand>
    {
        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new NotFoundException("No user found");

            _logger.LogInformation("Admin {AdminId} request to update order {OrderId} status to {NewStatus}", currentUser.Id, request.OrderId, request.Status);

            var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(
                new OrderSpecifications(orderId: request.OrderId, addOrderItems: true))
                ?? throw new NotFoundException($"No order found!");

            await _orderService.UpdateOrderStatusTransactionAsync(order, request.Status);
        }
    }
}
