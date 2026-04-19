using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Domain.Models
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string ShortName { get; set; } = default!;  // "Standard", "Express"
        public string Description { get; set; } = default!; // "Delivered in 3-5 days"
        public string DeliveryTime { get; set; } = default!; // "3-5 Business Days"

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }
    }
}
