using System.Reflection;
using Application.Features.Posts.Commands;
using Application.Features.Users.Command;
using Application.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Application;

public static  class DependencyInjection
{
    public static void AddApplication(this IServiceCollection service)
    {
        var assembly = Assembly.GetExecutingAssembly();
        service.AddMediatR(assembly);
        service.AddValidatorsFromAssembly(assembly);
    }

    public static void AddValidators(this IServiceCollection service)
    {
        service.AddScoped<IValidator<AddUserCommand>, AddUserCommandValidation>();
        service.AddScoped<IValidator<AddPostCommand>, AddPostCommandValidation>();
    }
}