namespace Talabat.Domain.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailConfirmationCodeAsync(string email, string firstName, string confirmationCode);
        Task SendPasswordResetCodeAsync(string email, string firstName, string resetCode);
    }
}
