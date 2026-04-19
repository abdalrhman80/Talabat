namespace Talabat.Application.Auth.Commands.ResendConfirmation
{
    internal class ResendConfirmationCommandHandler(
        ILogger<ResendConfirmationCommandHandler> _logger,
        UserManager<User> _userManager,
        IAuthService _authService,
        IEmailService _emailService
        ) : IRequestHandler<ResendConfirmationCommand>
    {
        public async Task Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new BadRequestException("Invalid user email.");

            if (user.EmailConfirmed)
                throw new BadRequestException("Email is already confirmed.");

            _logger.LogInformation("User {UserId} requested to resend email confirmation code", user.Id);

            var code = await _authService.GenerateConfirmationEmailCodeAsync(user);

            await _emailService.SendEmailConfirmationCodeAsync(user.Email!, user.FirstName!, code);
        }
    }
}
