using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data
{
    public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("auth");

            modelBuilder.Entity<User>(user => user.ToTable("Users", "auth"));

            modelBuilder.Entity<Role>(role =>
            {
                role.ToTable("Roles", "auth");
                //role.HasKey(r => r.Id);

                role.HasMany(r => r.UserRoles)
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.ToTable("UserRoles", "auth");

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(userClaim => userClaim.ToTable("UserClaims", "auth"));

            modelBuilder.Entity<IdentityUserLogin<string>>(userLogin => userLogin.ToTable("UserLogins", "auth"));

            modelBuilder.Entity<IdentityRoleClaim<string>>(roleClaim => roleClaim.ToTable("RoleClaims", "auth"));

            modelBuilder.Entity<IdentityUserToken<string>>(userToken => userToken.ToTable("UserTokens", "auth"));
        }
    }
}
