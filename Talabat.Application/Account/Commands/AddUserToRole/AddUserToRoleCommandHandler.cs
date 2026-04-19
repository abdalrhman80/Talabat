
namespace Talabat.Application.Account.Commands.AddUserToRole
{
    internal class AddUserToRoleCommandHandler(
        ILogger<AddUserToRoleCommandHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        UserManager<User> _userManager
        ) : IRequestHandler<AddUserToRoleCommand>
    {
        public async Task Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var adminUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(request.UserId))
                throw new BadRequestException("User ID is required.");

            if (string.IsNullOrEmpty(request.RoleName))
                throw new BadRequestException("Role Name is required.");

            var dbUser = await _userManager.FindByIdAsync(request.UserId) ?? throw new NotFoundException("User not found.");

            await _userRepository.AddUserToRoleAsync(dbUser, request.RoleName, cancellationToken);

            _logger.LogInformation("Admin {AdminId} adding user {UserIdToAdd} to role {RoleName}", adminUser.Id, request.UserId, request.RoleName);
        }
    }
}
