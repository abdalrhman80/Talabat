namespace Talabat.Application.Auth.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Email, string ConfirmationCode) : IRequest;
}
