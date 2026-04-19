using Microsoft.AspNetCore.Identity;

namespace Talabat.Domain.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; } = default!;
        public string? LastName { get; set; } = default!;
        public string? PicturePath { get; set; } = default!;
        public string? EmailConfirmationCode { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationCodeExpiresAt { get; set; }
        public string? PasswordResetCode { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetCodeExpiresAt { get; set; } 
        public Address? Address { get; set; } = new(); // owned entity
        public ICollection<UserRole> UserRoles { get; set; } = [];
        public List<RefreshToken>? RefreshTokens { get; set; } = [];
    }
}
