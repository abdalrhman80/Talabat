using MediatR;

namespace Talabat.Application.Account.Commands.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest;
}
