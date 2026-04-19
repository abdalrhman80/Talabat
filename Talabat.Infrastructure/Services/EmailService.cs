using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Talabat.Domain.Services;
using Talabat.Domain.Shared.Options;

namespace Talabat.Infrastructure.Services
{
    internal class EmailService(IOptions<EmailSettingsOptions> _emailSettingsOptions) : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailSettingsOptions.Value.SenderEmail),
                Subject = subject,
            };

            email.To.Add(MailboxAddress.Parse(toEmail));
            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_emailSettingsOptions.Value.DisplayName, _emailSettingsOptions.Value.SenderEmail));

            using var smtpClient = new SmtpClient();
            smtpClient.Connect(_emailSettingsOptions.Value.Host, _emailSettingsOptions.Value.Port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_emailSettingsOptions.Value.SenderEmail, _emailSettingsOptions.Value.Password);
            await smtpClient.SendAsync(email);
            smtpClient.Disconnect(true);

        }

        public async Task SendEmailConfirmationCodeAsync(string email, string firstName, string confirmationCode)
        {
            var subject = "Email Confirmation Code";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #2c3e50; margin-bottom: 10px;'>Email Confirmation Code</h1>
                        <div style='width: 50px; height: 3px; background-color: #3498db; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 30px; border-radius: 10px; margin-bottom: 20px;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Hi {firstName},</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            Thank you for registering with our Talabat platform! To complete your registration, please use the following confirmation code:
                        </p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='font-size: 24px; font-weight: bold; color: #3498db;'>{confirmationCode}</span>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            Please enter this code in the confirmation field on our app to verify your email address.
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br> 
                        </p>
                    </div>
                </div>
            ";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetCodeAsync(string email, string firstName, string resetCode)
        {
            var subject = "Password Reset Code";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #e74c3c; margin-bottom: 10px;'>Password Reset Code</h1>
                        <div style='width: 50px; height: 3px; background-color: #e74c3c; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 30px; border-radius: 10px; margin-bottom: 20px;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Hello {firstName},</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            You have requested to reset your password.
                        </p>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 25px;'>
                            Use the following code to reset your password:
                        </p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='font-size: 24px; font-weight: bold; color: #e74c3c;'>{resetCode}</span>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            Please enter this code in the confirmation field on our app to verify your email address.
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br> 
                        </p>
                    </div>
                </div>
            ";

            await SendEmailAsync(email, subject, body);
        }

    }
}
