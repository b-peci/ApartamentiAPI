using Application.Interfaces;
using Application.Interfaces.Helpers;
using Application.Interfaces.Repositories;
using Infrastructure.Implementations;
using Infrastructure.Implementations.Helpers;
using Infrastructure.Implementations.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection service)
    {
        #region Repositories
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddScoped<IPostRepository, PostRepository>();
        service.AddScoped<IRoleRepository, RoleRepository>();
        service.AddScoped<IImageRepository, ImageRepository>();
        service.AddScoped<IChatRepository, ChatRepository>();
        service.AddScoped<IReadPostRepository, ReadPostRepository>();
        service.AddScoped<ICityRepository, CityRepository>();
        service.AddScoped<ICountryRepository, CountryRepository>();
        #endregion

        service.AddScoped<IJWTToken, JWTToken>();
        service.AddScoped<IPassword, Password>();
        service.AddScoped<IImageHelper, ImageHelper>();
    }
}