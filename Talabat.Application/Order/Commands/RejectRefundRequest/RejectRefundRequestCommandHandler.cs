namespace Talabat.Application.Order.Commands.RejectRefundRequest
{
    internal class RejectRefundRequestCommandHandler(
        ILogger<RejectRefundRequestCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<RejectRefundRequestCommand>
    {
        public async Task Handle(RejectRefundRequestCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("Admin {AdminId} reject refund request {@request}", currentUser.Id, request);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var refundRequest = await _unitOfWork.Repository<RefundRequest>().GetEntityWithSpecificationAsync(
                    new RefundRequestSpecifications(requestId: request.RequestId, includeOrder: true))
                    ?? throw new NotFoundException($"No refund request found with id {request.RequestId}");

                if (refundRequest.Status != RefundRequestStatus.Pending)
                    throw new BadRequestException($"Refund request is already {refundRequest.Status}.");

                refundRequest.Status = RefundRequestStatus.Rejected;
                refundRequest.AdminNotes = request.Notes;
                refundRequest.ReviewedAt = DateTimeOffset.Now.ToLocalTime();

                refundRequest.Order.Status = OrderStatus.Success;

                _unitOfWork.Repository<RefundRequest>().Update(refundRequest);
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
