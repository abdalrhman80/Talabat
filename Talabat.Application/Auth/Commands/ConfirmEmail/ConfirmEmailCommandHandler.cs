namespace Talabat.Application.Auth.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler(
        ILogger<ConfirmEmailCommandHandler> _logger,
        UserManager<User> _userManager
        ) : IRequestHandler<ConfirmEmailCommand>
    {
        public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.ConfirmationCode.Trim()))
                throw new BadRequestException("Email and code are required");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.EmailConfirmationCode?.Trim() != request.ConfirmationCode || user.EmailConfirmationCodeExpiresAt < DateTime.UtcNow.ToLocalTime())
                throw new BadRequestException("Invalid or expired confirmation code.");

            if (user.EmailConfirmed)
                throw new BadRequestException("Email is already confirmed.");

            var result = await _userManager.ConfirmEmailAsync(user, user.EmailConfirmationToken!);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Email confirmation failed: {errors}");
            }

            user.EmailConfirmationCode = null;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationCodeExpiresAt = null;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Email {Email} confirmed successfully for user {UserId}", request.Email, user.Id);
        }
    }
}
