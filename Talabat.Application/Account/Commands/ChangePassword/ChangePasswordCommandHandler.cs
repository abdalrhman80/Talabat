namespace Talabat.Application.Account.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler(
        ILogger<ChangePasswordCommandHandler> _logger,
        IUserContext _userContext,
        UserManager<User> _userManager
        ) : IRequestHandler<ChangePasswordCommand>
    {
        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User is required.");

            var dbUser = await _userManager.FindByIdAsync(currentUser.Id) ?? throw new NotFoundException("User not found.");

            _logger.LogInformation("Changing password for user: {UserId}", dbUser.Id);

            var result = await _userManager.ChangePasswordAsync(dbUser, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
