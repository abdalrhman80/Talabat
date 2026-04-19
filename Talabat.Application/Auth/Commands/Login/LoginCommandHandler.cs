namespace Talabat.Application.Auth.Commands.Login
{
    internal class LoginCommandHandler(
        ILogger<LoginCommandHandler> _logger,
        IAuthService _authService,
        UserManager<User> _userManager)
        : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                //.Include(u => u.UserRoles)
                //.ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken) ?? throw new NotFoundException("No user found");

            if (user.Email != "admin@gmail.com" && !user.EmailConfirmed)
            {
                throw new BadRequestException("Email is not confirmed. Please confirm your email before logging in.");
            }

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new BadRequestException("Invalid email or password.");
            }

            if (user.PasswordResetCode != null)
            {
                throw new BadRequestException("Password reset is in progress. Please complete the password reset process.");
            }

            _logger.LogInformation("User {UserId} logged in", user.Id);

            return await _authService.GenerateAuthResultAsync(user);
        }
    }
}
