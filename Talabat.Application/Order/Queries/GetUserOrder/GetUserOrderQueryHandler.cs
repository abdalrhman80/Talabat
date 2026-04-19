namespace Talabat.Application.Order.Queries.GetUserOrder
{
    internal class GetUserOrderQueryHandler(
        ILogger<GetUserOrderQueryHandler> _logger,
        IUnitOfWork _unitOfWork,
        IUserContext _userContext,
        IMapper _mapper
        ) : IRequestHandler<GetUserOrderQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetUserOrderQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new NotFoundException("No user found");

            _logger.LogInformation("User {UserId} request to get his order {OrderId}", currentUser.Id, request.OrderId);

            var order = await _unitOfWork.Repository<UserOrder>().GetEntityWithSpecificationAsync(
                new OrderSpecifications(currentUser.Email, orderId: request.OrderId, addOrderItems: true, addDeliveryMethod: true))
                ?? throw new NotFoundException($"No order found!");

            return _mapper.Map<OrderDto>(order);
        }
    }
}
