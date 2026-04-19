namespace Talabat.Application.Account.Queries.GetUser
{
    internal class GetUserQueryHandler(
        ILogger<GetUserQueryHandler> _logger,
        IUserContext _userContext,
        UserManager<User> _userManager,
        IMapper _mapper
        ) : IRequestHandler<GetUserQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbUser = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(message: $"User with ID {currentUser.Id} not found.");

            _logger.LogInformation("User {UserId} requested his profile information.", dbUser.Id);

            return _mapper.Map<UserDto>(dbUser);
        }
    }
}
