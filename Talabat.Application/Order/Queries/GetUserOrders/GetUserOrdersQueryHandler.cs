namespace Talabat.Application.Order.Queries.GetUserOrders
{
    internal class GetUserOrdersQueryHandler(
        ILogger<GetUserOrdersQueryHandler> _logger,
        IUnitOfWork _unitOfWork,
        IUserContext _userContext,
        IMapper _mapper
        ) : IRequestHandler<GetUserOrdersQuery, PaginationResponse<OrderDto>>
    {
        public async Task<PaginationResponse<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new NotFoundException("No user found");

            _logger.LogInformation("User {UserId} request to get his orders with params {@Params}", currentUser.Id, request);

            var orderParams = new OrderParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                PaymentMethod = request.PaymentMethod,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                Status = request.Status
            };

            var orders = await _unitOfWork.Repository<UserOrder>().GetAllWithSpecificationAsync(new OrderSpecifications(currentUser.Email, orderParams));

            var result = new PaginationResponse<OrderDto>(orderParams.PageNumber, orderParams.PageSize, orders.Count, _mapper.Map<IReadOnlyList<OrderDto>>(orders) ?? []);

            return result;
        }
    }
}
