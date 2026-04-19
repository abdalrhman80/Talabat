namespace Talabat.Application.Auth.Commands.ForgetPassword
{
    public record ForgetPasswordCommand(string Email) : IRequest;
}
