namespace Talabat.Application.Account.Commands.UpdateUser
{
    public record UpdateUserCommand(string? FirstName, string? LastName, string? PhoneNumber, UserDto.AddressDto? Address) : IRequest<UserDto>;
}
