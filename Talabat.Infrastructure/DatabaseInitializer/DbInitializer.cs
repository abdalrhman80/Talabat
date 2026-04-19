using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Talabat.Domain.Models;
using Talabat.Domain.Repositories;
using Talabat.Domain.Services;
using Talabat.Domain.Shared.Constants;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.DatabaseInitializer
{
    internal class DbInitializer(
        AppDbContext _dbContext,
        AppIdentityDbContext _identityDbContext,
        UserManager<User> _userManager,
        RoleManager<Role> _roleManager,
        IUserRepository _userRepository) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            #region Update Database (Migrations)
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();

                if (_identityDbContext.Database.GetPendingMigrations().Any())
                    await _identityDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An Error Occurred During Appling The Migrations", ex);
            }
            #endregion

            #region Create Roles and Admin User
            //if (!await _identityDbContext.Roles.AnyAsync(r => r.Name == Roles.Admin))
            if (!await _roleManager.RoleExistsAsync(RoleNames.Admin))
            {
                await _roleManager.CreateAsync(new Role() { Name = RoleNames.Admin });
                await _roleManager.CreateAsync(new Role() { Name = RoleNames.Customer });

                var newAdminUser = new User
                {
                    FirstName = "Administrator",
                    LastName = "Admin",
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(newAdminUser, "Admin@123");

                var administrator = await _userManager.FindByEmailAsync(newAdminUser.Email);

                await _userRepository.AddUserToRoleAsync(administrator!, RoleNames.Admin);
            }
            #endregion

            #region Data Seeding
            var baseDirectory = "../Talabat.Infrastructure/DatabaseInitializer/SeedData";

            if (!await _dbContext.ProductBrands.AnyAsync())
            {
                try
                {
                    var jsonString = File.ReadAllText($"{baseDirectory}/ProductBrands.json");
                    var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(jsonString);

                    if (productBrands!.Count != 0)
                    {
                        foreach (var productBrand in productBrands)
                        {
                            await _dbContext.ProductBrands.AddAsync(productBrand);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("An Error Occurred During Seeding The ProductBrands", ex);
                }
            }

            if (!await _dbContext.ProductTypes.AnyAsync())
            {
                try
                {
                    var jsonString = File.ReadAllText($"{baseDirectory}/ProductTypes.json");
                    var productTypes = JsonSerializer.Deserialize<List<ProductType>>(jsonString);

                    if (productTypes!.Count != 0)
                    {
                        foreach (var productType in productTypes)
                        {
                            await _dbContext.ProductTypes.AddAsync(productType);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("An Error Occurred During Seeding The ProductTypes", ex);
                }
            }

            if (!await _dbContext.Products.AnyAsync())
            {
                try
                {
                    var jsonString = File.ReadAllText($"{baseDirectory}/Products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(jsonString);

                    if (products!.Count != 0)
                    {
                        foreach (var product in products)
                        {
                            await _dbContext.Products.AddAsync(product);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("An Error Occurred During Seeding The Products", ex);
                }
            }

            if(!await _dbContext.DeliveryMethods.AnyAsync())
            {
                try
                {
                    var jsonString = File.ReadAllText($"{baseDirectory}/DeliveryMethods.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(jsonString);

                    if (deliveryMethods!.Count != 0)
                    {
                        foreach (var deliveryMethod in deliveryMethods)
                        {
                            await _dbContext.DeliveryMethods.AddAsync(deliveryMethod);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("An Error Occurred During Seeding The DeliveryMethods", ex);
                }
            }
             
            #endregion
        }
    }
}
