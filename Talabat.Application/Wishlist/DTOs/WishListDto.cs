namespace Talabat.Application.Wishlist.DTOs
{
    public class WishListDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = default!;
        public string AddedAt { get; set; } = default!;
        public required WishListItemDto Item { get; set; }
    }

    public class WishListItemDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IList<WishListPictureDto>? Pictures { get; set; }
        public decimal Price { get; set; }
        public required string ProductBrand { get; set; }
        public required string ProductType { get; set; }

        public class WishListPictureDto
        {
            public int Id { get; set; }
            public required string PictureUrl { get; set; }
        }
    }
}
