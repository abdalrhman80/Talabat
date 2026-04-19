namespace Talabat.Application.Account.Resolver
{
    internal class UserImageUrlResolver(IConfiguration _configuration) : IValueResolver<User, UserDto, string>
    {
        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
            => !string.IsNullOrEmpty(source.PicturePath) ? $"{_configuration["BaseUrl"]}/{source.PicturePath}" : null!;
    }
}
