namespace Talabat.Application.Account.Commands.DeleteUser
{
    internal class DeleteUserCommandHandler(
        ILogger<DeleteUserCommandHandler> _logger,
        IUserContext _userContext,
        UserManager<User> _userManager
        ) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User ID is required.");

            var dbUser = await _userManager.FindByIdAsync(currentUser.Id) ?? throw new NotFoundException("User not found.");

            _logger.LogInformation("Deleting user: {UserId}", dbUser.Id);


            var result = await _userManager.DeleteAsync(dbUser);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
