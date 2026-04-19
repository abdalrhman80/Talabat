namespace Talabat.Application.Order.Queries.GetRefundRequestToAdmin
{
    internal class GetRefundRequestToAdminQueryHandler(
        ILogger<GetRefundRequestToAdminQueryHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetRefundRequestToAdminQuery, RefundRequestDto>
    {
        public async Task<RefundRequestDto> Handle(GetRefundRequestToAdminQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("Admin {AdminId} get refund request {requestId}", currentUser.Id, request.RequestId);

            var refundRequest = await _unitOfWork.Repository<RefundRequest>().GetEntityWithSpecificationAsync(
                new RefundRequestSpecifications(request.RequestId, includeOrder: true, includeOrderItems: true))
                ?? throw new NotFoundException($"No refund request found with id {request.RequestId}");

            return _mapper.Map<RefundRequestDto>(refundRequest);
        }
    }
}
