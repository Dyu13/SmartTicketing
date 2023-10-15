using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartTicketing.Application.Auth;
using System;

namespace SmartTicketing.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton(typeof(ILogger), provider => provider.GetService<ILogger<AuthService>>());

        return services;
    }
}