namespace Talabat.Application.Reviews.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.BuyerName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Review, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.DateTime.ToString("dddd, dd MMMM yyyy hh:mm tt")))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.DateTime.ToString("dddd, dd MMMM yyyy hh:mm tt") : null));
        }
    }

}