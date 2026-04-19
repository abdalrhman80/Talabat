using Talabat.Application.Auth.Commands.Register;

namespace Talabat.Application.Auth.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterCommand, User>();

        }
    }
}
