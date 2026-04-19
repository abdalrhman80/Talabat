namespace Talabat.Domain.Models
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!;
        public ICollection<BasketItem> BasketItems { get; set; } = [];

        public class BasketItem
        {
            public int Id { get; set; } = default!;
            public string Name { get; set; } = default!;
            public ItemPicture? Picture { get; set; }
            public string Type { get; set; } = default!;
            public string Brand { get; set; } = default!;
            public decimal Price { get; set; }
            public int Quantity { get; set; }

            public class ItemPicture
            {
                public int Id { get; set; }
                public string PictureUrl { get; set; } = default!;
            }
        }
    }
}
