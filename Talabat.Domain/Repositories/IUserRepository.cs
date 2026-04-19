using Talabat.Domain.Models;

namespace Talabat.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddUserToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default);
        Task RemoveUserFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default);
    }
}
