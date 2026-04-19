namespace Talabat.Application.Order.Queries.GeInvoice
{
    public record GeInvoiceQuery(int InvoiceId) : IRequest<InvoiceDto>;
}
