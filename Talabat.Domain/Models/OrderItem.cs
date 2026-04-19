using Microsoft.EntityFrameworkCore;

namespace Talabat.Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public ProductItemOrdered Product { get; set; } = new(); // owned
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = new();

        [Owned]
        public class ProductItemOrdered
        {
            public int Id { get; set; }
            public string Name { get; set; } = default!;
            public string Description { get; set; } = default!;
            public string PicturePath { get; set; } = default!;
        }
    }
}
