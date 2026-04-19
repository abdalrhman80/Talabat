namespace Talabat.Application.Order.Resolver
{
    public class OrderItemPictureUrlResolver(IConfiguration _configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
            => !string.IsNullOrEmpty(source.Product.PicturePath) ? $"{_configuration["ApiBaseUrl"]}/{source.Product.PicturePath}" : string.Empty;
    }
}
