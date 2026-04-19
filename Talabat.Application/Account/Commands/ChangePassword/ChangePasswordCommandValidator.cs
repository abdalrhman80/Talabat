namespace Talabat.Application.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .MinimumLength(8).WithMessage("Current password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("Current password must not exceed 100 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("New password must not exceed 100 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
                .WithMessage("New password must contain at least one uppercase letter, one lowercase letter, and one number.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.NewPassword).WithMessage("Confirm password must match the new password.")
                .MinimumLength(8).WithMessage("Confirm password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("Confirm password must not exceed 100 characters.");
        }
    }
}
