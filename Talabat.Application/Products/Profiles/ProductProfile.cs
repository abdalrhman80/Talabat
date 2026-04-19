using Talabat.Application.Products.Commands.CreateProduct;
using Talabat.Application.Products.Commands.UpdateProduct;
using Talabat.Application.Products.Resolver;

namespace Talabat.Application.Products.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.ProductPictures));

            CreateMap<ProductPicture, ProductPictureDto>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>();

            CreateMap<ProductType, ProductTypeDto>();

            CreateMap<ProductBrand, ProductBrandDto>();
        }
    }
}
