using Serilog;
using StackExchange.Redis;
using Talabat.Api.Middlewares;

namespace Talabat.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public void AddPresentation()
            {
                builder.Services.AddControllers();
                builder.Services.AddOpenApi();
                builder.Services.AddScoped<ExceptionHandlingMiddleware>();
                builder.Services.AddSerilog((services, loggerConfiguration) =>
                {
                    loggerConfiguration
                     .ReadFrom.Configuration(builder.Configuration)
                     .ReadFrom.Services(services);
                });

                // Register CORS
                builder.Services.AddCors(c => c.AddPolicy("AllowAny", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }));

                // Register Redis
                builder.Services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(builder.Configuration["RedisConnection"]!));
            }
        }
    }
}
