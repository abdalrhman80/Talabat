namespace Talabat.Application.Order.DTOs
{
    public class OrderWithInvoiceDto
    {
        public required OrderDto Order { get; set; }
        public required FawaterakBasePaymentResponse Invoice { get; set; }
        public required FawaterakBasePaymentDataResponse PaymentData { get; set; }
    }
}
