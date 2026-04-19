namespace Talabat.Application.Auth.Commands.ResendConfirmation
{
    public record ResendConfirmationCommand(string Email) : IRequest;
}
