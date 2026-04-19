using Talabat.Domain.Models;
using Talabat.Domain.Shared;

namespace Talabat.Domain.Services
{
    public interface IAuthService
    {
        Task<object> GenerateAccessTokenAsync(User user);
        Task<AuthenticationResponse> GenerateAuthResultAsync(User user);
        RefreshToken GenerateRefreshToken();
        Task<string> GenerateConfirmationEmailCodeAsync(User user);
        Task<string> GeneratePasswordResetCodeAsync(User user);
    }
}
