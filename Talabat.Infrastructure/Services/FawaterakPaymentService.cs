using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Talabat.Domain.Models;
using Talabat.Domain.Repositories;
using Talabat.Domain.Services;
using Talabat.Domain.Shared.Constants;
using Talabat.Domain.Shared.Exceptions;
using Talabat.Domain.Shared.Options;
using Talabat.Domain.Shared.Payment;
using Talabat.Domain.Specifications.EntitiesSpecifications;
using static Talabat.Domain.Shared.Payment.FawaterakPaymentMethodResponse;


namespace Talabat.Infrastructure.Services
{
    internal class FawaterakPaymentService(
        IHttpClientFactory _httpClientFactory,
        IOptions<FawaterakOptions> options,
        IUnitOfWork _unitOfWork
        ) : IFawaterakPaymentService
    {
        private readonly string ApiKey = options.Value.ApiKey;
        private readonly string FawaterkBaseUrl = options.Value.BaseUrl;
        private readonly string ProviderKey = options.Value.ProviderKey;

        public async Task<IReadOnlyList<FawaterakPaymentMethodResponseData>?> GetPaymentMethods()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{FawaterkBaseUrl}/getPaymentmethods");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = await client.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                var _response = JsonSerializer.Deserialize<FawaterakPaymentMethodResponse>(responseContent);
                return _response!.Data!;
            }

            return null;
        }

        public async Task<(FawaterakBasePaymentResponse?, FawaterakBasePaymentDataResponse? paymentData)?> Pay(FawaterakInvoiceRequestModel invoice)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{FawaterkBaseUrl}/invoiceInitPay");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
                request.Content = new StringContent(JsonSerializer.Serialize(invoice), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var method = await GetPaymentMethod(invoice.PaymentMethodId!.Value);

                    switch (method)
                    {
                        case PaymentMethodNames.Fawry:
                            {
                                var _fawryResponse = JsonSerializer.Deserialize<FawryPaymentResponse>(responseContent);

                                return _fawryResponse!.Data.PaymentData.Error != null
                                    ? throw new Exception($"Fawry Payment Error: {_fawryResponse.Data.PaymentData.Error}")
                                    : ((FawaterakBasePaymentResponse)_fawryResponse!.Data, _fawryResponse.Data.PaymentData);
                            }
                        case PaymentMethodNames.EWallet:
                            {
                                var _meezaResponse = JsonSerializer.Deserialize<MeezaPaymentResponse>(responseContent);

                                return _meezaResponse!.Data.PaymentData.Error != null
                                    ? throw new Exception($"Meeza Payment Error: {_meezaResponse.Data.PaymentData.Error}")
                                    : ((FawaterakBasePaymentResponse)_meezaResponse!.Data, _meezaResponse.Data.PaymentData);
                            }
                        case PaymentMethodNames.Card:
                            {
                                var _cardResponse = JsonSerializer.Deserialize<CardPaymentResponse>(responseContent);

                                return _cardResponse!.Data.PaymentData.Error != null
                                    ? throw new Exception($"Card Payment Error: {_cardResponse.Data.PaymentData.Error}")
                                    : ((FawaterakBasePaymentResponse)_cardResponse!.Data, _cardResponse.Data.PaymentData);
                            }
                        default:
                            return null;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FawaterakInvoiceResponse?> GetInvoiceAsync(string buyerEmail, int invoiceId)
        {
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationAsync(new OrderSpecifications(buyerEmail, invoiceId.ToString()))
                ?? throw new NotFoundException($"Order with Invoice ID {invoiceId} not found.");

            if (order.Status == OrderStatus.Cancelled) throw new BadRequestException("Cannot retrieve invoice for a cancelled order.");

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{FawaterkBaseUrl}/getInvoiceData/{invoiceId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FawaterakInvoiceResponse>(responseContent);
        }

        public async Task<bool> VerifyPaidWebhook(SuccessWebHookModel model)
        {
            var queryParam = $"InvoiceId={model.InvoiceId}&InvoiceKey={model.InvoiceKey}&PaymentMethod={model.PaymentMethod}";
            var computedHash = GenerateHashKey(queryParam);
            return computedHash.Equals(model.HashKey, StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<bool> VerifyCancelWebhook(CancelWebHookModel model)
        {
            var queryParam = $"referenceId={model.ReferenceId}&PaymentMethod={model.PaymentMethod}";
            var computedHash = GenerateHashKey(queryParam);
            return computedHash.Equals(model.HashKey, StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<bool> VerifyFailedWebhook(FailedWebHookModel model)
        {
            var queryParam = $"InvoiceId={model.InvoiceId}&InvoiceKey={model.InvoiceKey}&PaymentMethod={model.PaymentMethod}";
            var computedHash = GenerateHashKey(queryParam);
            return computedHash.Equals(model.HashKey, StringComparison.CurrentCultureIgnoreCase);
        }


        #region Helper Methods

        private async Task<PaymentMethodNames> GetPaymentMethod(int paymentMethodId, IReadOnlyList<FawaterakPaymentMethodResponseData>? paymentMethods = null)
        {
            var methods = paymentMethods ?? await GetPaymentMethods();

            var method = methods?.FirstOrDefault(x => x.PaymentId == paymentMethodId);

            if (string.IsNullOrWhiteSpace(method?.NameEn))
                return PaymentMethodNames.Card;

            var name = method.NameEn;

            if (name.Contains("Fawry", StringComparison.OrdinalIgnoreCase))
                return PaymentMethodNames.Fawry;

            if (name.Contains("Meeza", StringComparison.OrdinalIgnoreCase) || name.Contains("Wallet", StringComparison.OrdinalIgnoreCase))
                return PaymentMethodNames.EWallet;

            return PaymentMethodNames.Card;
        }

        private string GenerateHashKey(string queryParam)
        {
            var keyBytes = Encoding.UTF8.GetBytes(ApiKey);
            var messageBytes = Encoding.UTF8.GetBytes(queryParam);
            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return Convert.ToHexStringLower(hashBytes);
        }

        #endregion
    }
}
