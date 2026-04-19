using Talabat.Api.Extensions;
using Talabat.Api.Middlewares;
using Talabat.Infrastructure.Extensions;
using Talabat.Application.Extensions;
using Talabat.Domain.Services;


var builder = WebApplication.CreateBuilder(args);

#region DI container
builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
#endregion

var app = builder.Build();

#region Initialize Database
using var scope = app.Services.CreateScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
await dbInitializer.InitializeAsync();
#endregion

#region Middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowAny");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion