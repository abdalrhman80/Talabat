using Talabat.Domain.Shared.Payment;
using static Talabat.Domain.Shared.Payment.FawaterakPaymentMethodResponse;

namespace Talabat.Domain.Services
{
    public interface IFawaterakPaymentService
    {
        Task<IReadOnlyList<FawaterakPaymentMethodResponseData>?> GetPaymentMethods();
        Task<(FawaterakBasePaymentResponse?, FawaterakBasePaymentDataResponse? paymentData)?> Pay(FawaterakInvoiceRequestModel invoice);
        Task<FawaterakInvoiceResponse?> GetInvoiceAsync(string buyerEmail, int invoiceId);
        Task<bool> VerifyPaidWebhook(SuccessWebHookModel model);
        Task<bool> VerifyCancelWebhook(CancelWebHookModel model);
        Task<bool> VerifyFailedWebhook(FailedWebHookModel model);
    }
}
