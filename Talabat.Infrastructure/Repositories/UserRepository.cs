using Microsoft.EntityFrameworkCore;
using Talabat.Domain.Models;
using Talabat.Domain.Repositories;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Repositories
{
    internal class UserRepository(AppIdentityDbContext _identityDbContext) : IUserRepository
    {
        public async Task AddUserToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
        {
            var role = await _identityDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken: cancellationToken) ??
                throw new InvalidOperationException(message: $"Role {roleName} does not exist");

            var existingUserRole = await _identityDbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken: cancellationToken);

            if (existingUserRole != null)
                throw new InvalidOperationException($"User {user.Id} is already in role {roleName}");

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                User = user,
                Role = role
            };

            _identityDbContext.UserRoles.Add(userRole);
            await _identityDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
        {
            var role = await _identityDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken: cancellationToken) ??
                throw new InvalidOperationException(message: $"Role {roleName} does not exist");

            var userRole = await _identityDbContext.Set<UserRole>().FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException($"User {user.Id} not in {roleName}");

            _identityDbContext.Set<UserRole>().Remove(userRole);
            await _identityDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
