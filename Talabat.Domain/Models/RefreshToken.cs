using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Domain.Models
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpirationOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
