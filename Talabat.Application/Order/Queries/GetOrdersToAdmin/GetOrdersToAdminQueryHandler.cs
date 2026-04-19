namespace Talabat.Application.Order.Queries.GetOrdersToAdmin
{
    internal class GetOrdersToAdminQueryHandler(
        ILogger<GetOrdersToAdminQueryHandler> _logger,
        IUnitOfWork _unitOfWork,
        IUserContext _userContext,
        IMapper _mapper
        ) : IRequestHandler<GetOrdersToAdminQuery, PaginationResponse<OrderDto>>
    {
        public async Task<PaginationResponse<OrderDto>> Handle(GetOrdersToAdminQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new NotFoundException("No user found");

            _logger.LogInformation("Admin {UserId} request to get all orders with params {@Params}", currentUser.Id, request);

            var orderParams = new OrderParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                PaymentMethod = request.PaymentMethod,
                SortBy = request.SortBy,
                SortDirection = request.SortDirection,
                Status = request.Status
            };

            var orders = await _unitOfWork.Repository<UserOrder>().GetAllWithSpecificationAsync(new OrderSpecifications(orderParams));

            var result = new PaginationResponse<OrderDto>(orderParams.PageNumber, orderParams.PageSize, orders.Count, _mapper.Map<IReadOnlyList<OrderDto>>(orders) ?? []);

            return result;
        }
    }
}
