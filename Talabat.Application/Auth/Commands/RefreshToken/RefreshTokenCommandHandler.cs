namespace Talabat.Application.Auth.Commands.RefreshToken
{
    internal class RefreshTokenCommandHandler(
        ILogger<RefreshTokenCommandHandler> _logger,
        UserManager<User> _userManager,
        IAuthService _authService
        ) : IRequestHandler<RefreshTokenCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
                throw new BadRequestException("Token is required!");

            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == request.Token), cancellationToken: cancellationToken)
                ?? throw new BadRequestException("Invalid Refresh Token");

            var refreshToken = user.RefreshTokens!.SingleOrDefault(t => t.Token == request.Token)!;

            if (!refreshToken.IsActive)
                throw new BadRequestException("Token is inactive or expired.");

            refreshToken.RevokedOn = DateTime.UtcNow.ToLocalTime();

            var newRefreshToken = _authService.GenerateRefreshToken();

            user.RefreshTokens!.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} refresh his token successfully.", user.Id);

            return await _authService.GenerateAuthResultAsync(user);
        }
    }
}
