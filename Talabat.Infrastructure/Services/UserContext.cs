using Microsoft.AspNetCore.Http;
using Talabat.Domain.Services;
using Talabat.Domain.Shared;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Infrastructure.Services
{
    internal class UserContext(IHttpContextAccessor _httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("User context is not present.");

            if (user.Identity is null || !user.Identity.IsAuthenticated) return null;

            var id = user.FindFirst(Claims.UserId)!.Value;
            var userName = user.FindFirst(Claims.UserName)!.Value;
            var firstName = user.FindFirst(Claims.FirstName)?.Value ?? string.Empty;
            var lastName = user.FindFirst(Claims.LastName)?.Value ?? string.Empty;
            var email = user.FindFirst(Claims.Email)?.Value ?? string.Empty;
            var phone = user.FindFirst(Claims.Phone)?.Value ?? string.Empty;
            var roles = user.FindAll(Claims.Role).Select(r => r.Value);

            return new CurrentUser(id, userName, firstName, lastName, email, phone, roles);
        }
    }
}
