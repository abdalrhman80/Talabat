using static Talabat.Domain.Shared.Payment.FawaterakPaymentMethodResponse;

namespace Talabat.Application.Order.Queries.PaymentMethods
{
    public class GetPaymentMethodsQuery : IRequest<IReadOnlyList<FawaterakPaymentMethodResponseData>>
    {
    }
}
