namespace Talabat.Application.Account.Commands.RemoveUserFromRole
{
    public record RemoveUserFromRoleCommand(string UserId, string RoleName) : IRequest;
}
