namespace Talabat.Domain.Models
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }
        public IList<Product> Products { get; set; } = [];
    }
}
