namespace Talabat.Application.Order.DTOs
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public required string InvoiceKey { get; set; }
        public required string PaymentMethod { get; set; }
        public required string Currency { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPaid { get; set; }
        public required string StatusText { get; set; }
        public required string CustomerEmail { get; set; }
        public required string CreatedAt { get; set; }
        public string? PaidAt { get; set; }
        public List<InvoiceTransactionDto> Transactions { get; set; } = [];
    }

    public class InvoiceTransactionDto
    {
        public required string TransactionId { get; set; }
        public string? ReferenceId { get; set; }
        public required string Status { get; set; }
        public required string Amount { get; set; }
        public required string PaidAt { get; set; }
        public required string PaymentMethod { get; set; }
    }
}
