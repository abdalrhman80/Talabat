namespace Talabat.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(3).WithMessage("First name must be at least 3 characters.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Please provide a valid phone number.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");

            RuleFor(p => p.ConfirmPassword)
                .Equal(p => p.Password).WithMessage("Passwords do not match.");

        }
    }
}
