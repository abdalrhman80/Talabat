namespace Talabat.Application.Wishlist.Resolver
{
    public class WishListPictureUrlResolver(IConfiguration _configuration) : IValueResolver<ProductPicture, WishListPictureDto, string>
    {
        public string Resolve(ProductPicture source, WishListPictureDto destination, string destMember, ResolutionContext context) 
            => !string.IsNullOrEmpty(source.PicturePath) ? $"{_configuration["ApiBaseUrl"]}/{source.PicturePath}" : string.Empty;
    }
}
