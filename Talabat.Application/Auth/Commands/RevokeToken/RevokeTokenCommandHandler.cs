namespace Talabat.Application.Auth.Commands.RevokeToken
{
    internal class RevokeTokenCommandHandler(
        ILogger<RevokeTokenCommandHandler> _logger,
        UserManager<User> _userManager)
        : IRequestHandler<RevokeTokenCommand>
    {
        public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
                throw new BadRequestException("Token is required!");

            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == request.Token))
                ?? throw new BadRequestException("Invalid Refresh Token");

            var refreshToken = user.RefreshTokens!.Single(t => t.Token == request.Token);

            if (!refreshToken.IsActive)
                throw new BadRequestException("Token is inactive or expired");

            refreshToken.RevokedOn = DateTime.UtcNow.ToLocalTime();

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} revoked his token.", user.Id);

        }
    }

}
