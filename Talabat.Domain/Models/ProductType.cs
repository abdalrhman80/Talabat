namespace Talabat.Domain.Models
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }
        public IList<Product> Products { get; set; } = [];
    }
}
