using Talabat.Application.Order.Resolver;

namespace Talabat.Application.Order.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>()
             .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.ProductPictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<UserOrder, OrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.NameEn))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.DateTime.ToString("dddd, dd MMMM yyyy hh:mm tt")))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryMethodCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.DateTime.ToString("dddd, dd MMMM yyyy hh:mm tt") : null))
                .ReverseMap();

            CreateMap<FawaterakInvoiceData, InvoiceDto>()
                 .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.InvoiceTransactions))
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Parse(src.CreatedAt).ToString("dddd, dd MMMM yyyy hh:mm tt")))
                 .ForMember(dest => dest.PaidAt, opt => opt.MapFrom(src => src.PaidAt != null ? DateTime.Parse(src.PaidAt).ToString("dddd, dd MMMM yyyy hh:mm tt") : null));

            CreateMap<FawaterakInvoiceTransaction, InvoiceTransactionDto>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId.ToString()))
                .ForMember(dest => dest.PaidAt, opt => opt.MapFrom(src => src.PaidAt != null
                    ? DateTime.Parse(src.PaidAt).ToString("dddd, dd MMMM yyyy hh:mm tt") : null));

            CreateMap<RefundRequest, RefundRequestDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => src.RequestedAt.ToString("dddd, dd MMMM yyyy hh:mm tt")))
                .ForMember(dest => dest.ReviewedAt, opt => opt.MapFrom(src => src.ReviewedAt.HasValue ? src.ReviewedAt.Value.ToString("dddd, dd MMMM yyyy hh:mm tt") : null));
        }
    }
}
