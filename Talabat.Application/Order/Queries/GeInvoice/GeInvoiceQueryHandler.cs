namespace Talabat.Application.Order.Queries.GeInvoice
{
    internal class GeInvoiceQueryHandler(
        ILogger<GeInvoiceQueryHandler> _logger,
        IUserContext _userContext,
        IFawaterakPaymentService _paymentService,
        IMapper _mapper
        ) : IRequestHandler<GeInvoiceQuery, InvoiceDto>
    {
        public async Task<InvoiceDto> Handle(GeInvoiceQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} request to get order {OrderId} invoice", currentUser.Id, request.InvoiceId);

            var invoice = await _paymentService.GetInvoiceAsync(currentUser.Email, request.InvoiceId) ?? throw new NotFoundException($"No invoice found with id {request.InvoiceId}");

            return _mapper.Map<InvoiceDto>(invoice.Data);
        }
    }
}
