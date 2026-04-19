using Talabat.Application.Wishlist.Resolver; 

namespace Talabat.Application.Wishlist.Profiles
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            CreateMap<WishListItem, WishListDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.AddedAt.DateTime.ToString("dddd, dd MMMM yyyy hh:mm tt")));

            CreateMap<Product, WishListItemDto>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.ProductPictures));

            CreateMap<ProductPicture, WishListPictureDto>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<WishListPictureUrlResolver>());
        }
    }
}
