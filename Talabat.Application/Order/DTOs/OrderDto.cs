using System.Text.Json.Serialization;

namespace Talabat.Application.Order.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public required string OrderDate { get; set; }
        public string? UpdatedAt { get; set; }
        public required string Status { get; set; }
        public required ShippingAddressDto ShippingAddress { get; set; } 

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public required ICollection<OrderItemDto> OrderItems { get; set; }
        public required string PaymentMethod { get; set; }
        public decimal SubTotal { get; set; }
        public required string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public required string InvoiceId { get; set; }

        public class ShippingAddressDto
        {
            public required string Street { get; set; }
            public required string City { get; set; }
            public required string State { get; set; }
            public required string Country { get; set; }
        }

        public class OrderItemDto
        {
            public int ProductId { get; set; }
            public required string ProductName { get; set; }
            public required string ProductDescription { get; set; }
            public required string ProductPictureUrl { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

    }
}

