namespace Talabat.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthenticationResponse>;
}
