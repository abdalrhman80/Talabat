namespace Talabat.Application.Auth.Commands.ResetPassword
{
    internal class ResetPasswordCommandHandler(
        ILogger<ResetPasswordCommandHandler> _logger,
        UserManager<User> _userManager
        ) : IRequestHandler<ResetPasswordCommand>
    {
        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken)
                ?? throw new BadRequestException("Invalid user email.");

            if (!user.EmailConfirmed || user.PasswordResetCode != request.PasswordResetCode || user.PasswordResetCodeExpiresAt < DateTime.UtcNow.ToLocalTime())
                throw new BadRequestException("Invalid or expired password reset code.");

            var result = await _userManager.ResetPasswordAsync(user, user.PasswordResetToken!, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Password reset failed: {errors}");
            }

            _logger.LogInformation("Password reset successfully for user {UserId}", user.Id);

            user.PasswordResetCode = null;
            user.PasswordResetToken = null;
            user.PasswordResetCodeExpiresAt = null;
            await _userManager.UpdateAsync(user);

            // RevokeAllUserTokens 
            var userRefreshTokens = user.RefreshTokens?.Where(t => t.IsActive).ToList() ?? [];

            userRefreshTokens.ForEach(t =>
            {
                t.RevokedOn = DateTime.UtcNow.ToLocalTime();
            });

            user.RefreshTokens = userRefreshTokens;

            await _userManager.UpdateAsync(user);
        }
    }
}
