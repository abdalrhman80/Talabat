using System.Security.Claims;

namespace Talabat.Domain.Shared.Constants
{
    public static class Claims
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string Email = "Email";
        public const string Phone = "Phone";
        public const string Role = ClaimTypes.Role;
    }
}
