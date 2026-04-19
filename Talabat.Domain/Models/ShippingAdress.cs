using Microsoft.EntityFrameworkCore;

namespace Talabat.Domain.Models
{
    [Owned]
    public class ShippingAddress
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
    }
}
