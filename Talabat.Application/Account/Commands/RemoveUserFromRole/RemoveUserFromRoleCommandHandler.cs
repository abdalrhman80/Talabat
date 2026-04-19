namespace Talabat.Application.Account.Commands.RemoveUserFromRole
{
    internal class RemoveUserFromRoleCommandHandler(
        ILogger<RemoveUserFromRoleCommandHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        UserManager<User> _userManager
        ) : IRequestHandler<RemoveUserFromRoleCommand>
    {
        public async Task Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            var adminUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(request.UserId))
                throw new BadRequestException("User ID is required.");

            if (string.IsNullOrEmpty(request.RoleName))
                throw new BadRequestException("Role Name is required.");

            var dbUser = await _userManager.FindByIdAsync(request.UserId) ?? throw new NotFoundException("User not found.");

            _logger.LogInformation("Admin {AdminId} is removing user {UserIdToRemove} from role {RoleName}", adminUser.Id, request.UserId, request.RoleName);

            await _userRepository.RemoveUserFromRoleAsync(dbUser, request.RoleName, cancellationToken);
        }
    }
}
