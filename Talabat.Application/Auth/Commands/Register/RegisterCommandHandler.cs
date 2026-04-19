namespace Talabat.Application.Auth.Commands.Register
{
    internal class RegisterCommandHandler(
        UserManager<User> _userManager,
        ILogger<RegisterCommandHandler> _logger,
        IMapper _mapper,
        IUserRepository _userRepository,
        IAuthService _authService,
        IEmailService _emailService
        ) : IRequestHandler<RegisterCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
            {
                throw new BadRequestException("Email Is Already Registered.");
            }

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
            {
                throw new BadRequestException("Username Is Already Taken.");
            }

            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"User registration failed: {errors}");
            }

            await _userRepository.AddUserToRoleAsync(user, RoleNames.Customer, cancellationToken: cancellationToken);

            _logger.LogInformation("New user registered successfully {@User}", new { request.FirstName, request.LastName, request.UserName, request.Email });

            var code = await _authService.GenerateConfirmationEmailCodeAsync(user);

            await _emailService.SendEmailConfirmationCodeAsync(user.Email!, user.FirstName!, code);

            return new AuthenticationResponse
            {
                Message = "User registered successfully.",
                UserName = user.UserName!,
                Email = user.Email!,
            };
        }
    }
}
