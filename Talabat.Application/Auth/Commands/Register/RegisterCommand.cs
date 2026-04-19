namespace Talabat.Application.Auth.Commands.Register
{
    public record RegisterCommand(string FirstName, string LastName, string UserName, string Email, string PhoneNumber, string Password, string ConfirmPassword) : IRequest<AuthenticationResponse>;
}
