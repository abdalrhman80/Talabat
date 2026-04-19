namespace Talabat.Application.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Email, string PasswordResetCode, string NewPassword) : IRequest;
}
