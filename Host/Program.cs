using System.Text;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Apartamenti API",
        Version = "v1"
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        configure => configure
            .WithOrigins(builder.Configuration["Links:UI"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition")
    );
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddValidators();
#endregion

var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
app.ConfigureAPI();
app.AddMapping();
using var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(new JsonFormatter(),
        "important.json",
        restrictedToMinimumLevel: LogEventLevel.Warning
    )
    .MinimumLevel.Debug()
    .CreateLogger();
try
{
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw e;
}
