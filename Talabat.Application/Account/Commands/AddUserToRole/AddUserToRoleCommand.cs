using MediatR;

namespace Talabat.Application.Account.Commands.AddUserToRole
{
    public record AddUserToRoleCommand(string UserId, string RoleName) : IRequest;
}
