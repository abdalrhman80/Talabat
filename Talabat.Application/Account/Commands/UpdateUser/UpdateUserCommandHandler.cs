namespace Talabat.Application.Account.Commands.UpdateUser
{
    internal class UpdateUserCommandHandler(
        ILogger<UpdateUserCommandHandler> _logger,
        IUserContext _userContext,
        IMapper _mapper,
        UserManager<User> _userManager
        ) : IRequestHandler<UpdateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User ID is required.");

            var dbUser = await _userManager.FindByIdAsync(currentUser.Id) ?? throw new NotFoundException("User not found.");

            _logger.LogInformation("Updating user: {UserId}, with {@Request}", dbUser.Id, request);

            dbUser.FirstName = request.FirstName ?? dbUser.FirstName;
            dbUser.LastName = request.LastName ?? dbUser.LastName;
            dbUser.PhoneNumber = request.PhoneNumber ?? dbUser.PhoneNumber;
            dbUser.Address = _mapper.Map<Address>(request.Address) ?? dbUser.Address;

            var result = await _userManager.UpdateAsync(dbUser);

            return result.Succeeded ? _mapper.Map<UserDto>(dbUser) : throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));
        }
    }
}
