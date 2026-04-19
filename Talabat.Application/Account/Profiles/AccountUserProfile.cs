using Talabat.Application.Account.Resolver;
using static Talabat.Application.Account.DTOs.UserDto;

namespace Talabat.Application.Account.Profiles
{
    public class AccountUserProfile : Profile
    {
        public AccountUserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<UserImageUrlResolver>())
                .ReverseMap();

            CreateMap<Address, AddressDto>()
                .ReverseMap();
        }
    }
}
