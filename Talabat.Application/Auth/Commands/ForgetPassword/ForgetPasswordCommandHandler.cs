namespace Talabat.Application.Auth.Commands.ForgetPassword
{
    internal class ForgetPasswordCommandHandler(
        ILogger<ForgetPasswordCommandHandler> _logger,
        UserManager<User> _userManager,
        IAuthService _authService,
        IEmailService _emailService
        ) : IRequestHandler<ForgetPasswordCommand>
    {
        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new BadRequestException("Email not exists");

            if (!user.EmailConfirmed)
                throw new BadRequestException("Please confirm your email first.");

            var code = await _authService.GeneratePasswordResetCodeAsync(user);

            _logger.LogInformation("Password reset code sent to user {UserId} to his email", user.Id);

            await _emailService.SendPasswordResetCodeAsync(user.Email!, user.FirstName!, code);
        }
    }
}
