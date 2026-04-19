namespace Talabat.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token) : IRequest<AuthenticationResponse>;
}
