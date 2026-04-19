using Microsoft.EntityFrameworkCore;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now.ToLocalTime();
        public DateTimeOffset? UpdatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ShippingAddress ShippingAddress { get; set; } = new(); // Owned
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal SubTotal { get; set; }
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public decimal GetShipping() => DeliveryMethod.Cost;
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public FawaterakPaymentMethod PaymentMethod { get; set; } = new();
        public string InvoiceId { get; set; } = "";


        [Owned]
        public class FawaterakPaymentMethod
        {
            public int Id { get; set; } = default!;
            public string NameEn { get; set; } = default!;
            public string NameAr { get; set; } = default!;
            public string Logo { get; set; } = default!;
            public bool Redirect { get; set; }
        }
    }
}
