namespace Talabat.Application.Order.Queries.GetRefundRequest
{
    internal class GetRefundRequestQueryHandler(
        ILogger<GetRefundRequestQueryHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetRefundRequestQuery, RefundRequestDto>
    {
        public async Task<RefundRequestDto> Handle(GetRefundRequestQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} get his refund request on order {OrderId}", currentUser.Id, request.OrderId);

            var refundRequest = await _unitOfWork.Repository<RefundRequest>().GetEntityWithSpecificationAsync(new RefundRequestSpecifications(currentUser.Email, request.OrderId))
                ?? throw new NotFoundException($"No refund request found for order {request.OrderId}");

            return _mapper.Map<RefundRequestDto>(refundRequest);
        }
    }
}
