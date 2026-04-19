namespace Talabat.Application.Products.Resolver
{
    public class ProductPictureUrlResolver(IConfiguration _configuration) : IValueResolver<ProductPicture, ProductPictureDto, string>
    {
        public string Resolve(ProductPicture source, ProductPictureDto destination, string destMember, ResolutionContext context) 
            => !string.IsNullOrEmpty(source.PicturePath) ? $"{_configuration["ApiBaseUrl"]}/{source.PicturePath}" : string.Empty;
    }
}
