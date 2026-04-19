namespace Talabat.Application.Auth.Commands.RevokeToken
{
    public record RevokeTokenCommand(string Token) : IRequest;
}
