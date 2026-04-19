using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Talabat.Application.Account.Commands.ChangePassword;
using Talabat.Application.Account.Profiles;
using Talabat.Application.Account.Resolver;
using Talabat.Application.Auth.Commands.Register;
using Talabat.Application.Auth.Commands.ResetPassword;
using Talabat.Application.Auth.Profiles;
using Talabat.Application.Order.Commands.CreateOrder;
using Talabat.Application.Order.Profiles;
using Talabat.Application.Order.Resolver;
using Talabat.Application.Products.Commands.CreateProduct;
using Talabat.Application.Products.Commands.UpdateProduct;
using Talabat.Application.Products.Profiles;
using Talabat.Application.Products.Resolver;
using Talabat.Application.Reviews.Profiles;
using Talabat.Application.Wishlist.Profiles;
using Talabat.Application.Wishlist.Resolver;

namespace Talabat.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public void AddApplication()
            {
                // Register MediatR
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

                // Register AutoMapper 
                services.AddAutoMapper(cfg =>
                {
                    cfg.AddProfile<AuthProfile>();
                    cfg.AddProfile<AccountUserProfile>();
                    cfg.AddProfile<ProductProfile>();
                    cfg.AddProfile<ReviewProfile>();
                    cfg.AddProfile<WishlistProfile>();
                    cfg.AddProfile<OrderProfile>();
                });


                // Resgiter Resolvers
                services.AddTransient<UserImageUrlResolver>();
                services.AddTransient<ProductPictureUrlResolver>();
                services.AddTransient<WishListPictureUrlResolver>();
                services.AddTransient<OrderItemPictureUrlResolver>();

                // Register FluentValidation
                services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
                services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();
                services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
                services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
                services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
                services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();


                services.AddFluentValidationAutoValidation(options =>
                {
                    //options.DisableBuiltInModelValidation = true;
                    options.ValidationStrategy = ValidationStrategy.All; // Use FluentValidation for model validation
                });
            }
        }
    }
}
