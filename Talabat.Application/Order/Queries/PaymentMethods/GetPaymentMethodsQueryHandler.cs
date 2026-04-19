using static Talabat.Domain.Shared.Payment.FawaterakPaymentMethodResponse;

namespace Talabat.Application.Order.Queries.PaymentMethods
{
    internal class GetPaymentMethodsQueryHandler(
         IFawaterakPaymentService _paymentService
        ) : IRequestHandler<GetPaymentMethodsQuery, IReadOnlyList<FawaterakPaymentMethodResponseData>>
    {
        public async Task<IReadOnlyList<FawaterakPaymentMethodResponseData>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            var paymentMethods = await _paymentService.GetPaymentMethods();
            return paymentMethods ?? throw new NotFoundException("No payment methods found!");
        }
    }
}
