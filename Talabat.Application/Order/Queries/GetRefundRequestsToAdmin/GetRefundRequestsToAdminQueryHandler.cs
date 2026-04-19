namespace Talabat.Application.Order.Queries.GetRefundRequestsToAdmin
{
    internal class GetRefundRequestsToAdminQueryHandler(
        ILogger<GetRefundRequestsToAdminQueryHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetRefundRequestsToAdminQuery, PaginationResponse<RefundRequestDto>>
    {
        public async Task<PaginationResponse<RefundRequestDto>> Handle(GetRefundRequestsToAdminQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("Admin {AdminId} get all refund requests witt params {@requestParams}", currentUser.Id, request);

            var requestParams = new RefundRequestParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                RequestStatus = request.RequestStatus,
                IncludeOrderItems = request.IncludeOrderItems,
            };

            var requests = await _unitOfWork.Repository<RefundRequest>().GetAllWithSpecificationAsync(new RefundRequestSpecifications(requestParams)) ?? [];

            var result = new PaginationResponse<RefundRequestDto>(
                requestParams.PageNumber,
                requestParams.PageSize,
                requests.Count,
                _mapper.Map<IReadOnlyList<RefundRequestDto>>(requests));

            return result;
        }
    }
}
