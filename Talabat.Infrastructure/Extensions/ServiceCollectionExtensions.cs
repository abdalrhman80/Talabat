using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Domain.Models;
using Talabat.Domain.Repositories;
using Talabat.Domain.Services;
using Talabat.Domain.Shared.Options;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.DatabaseInitializer;
using Talabat.Infrastructure.Repositories;
using Talabat.Infrastructure.Services;
using Role = Talabat.Domain.Models.Role;

namespace Talabat.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public void AddInfrastructure(IConfiguration configuration)
            {
                // Configure Entity Framework Core with SQL 
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });

                services.AddDbContext<AppIdentityDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });

                // Configure Identity
                services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;

                })
                   .AddRoles<Role>()
                   .AddEntityFrameworkStores<AppIdentityDbContext>()
                   .AddDefaultTokenProviders();


                // Configure options
                services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
                services.Configure<EmailSettingsOptions>(configuration.GetSection("EmailSettings"));
                services.Configure<FawaterakOptions>(configuration.GetSection("Fawaterak"));


                // Configure JWT
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).
                AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        ValidAudience = configuration["JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!)),
                    };
                });


                // Register repositories and unit of work
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IUserRepository, UserRepository>();

                // Register services
                services.AddScoped<IDbInitializer, DbInitializer>();
                services.AddScoped<IFileService, FileService>();
                services.AddScoped<IEmailService, EmailService>();
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<IBasketService, BasketService>();
                services.AddScoped<IOrderService, OrderService>();
                services.AddScoped<IFawaterakPaymentService, FawaterakPaymentService>();

                // Register IHttpContextAccessor
                services.AddScoped<IUserContext, UserContext>();
                services.AddHttpContextAccessor();

                services.AddHttpClient();

            }
        }
    }
}