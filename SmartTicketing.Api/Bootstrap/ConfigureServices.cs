using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

using SmartTicketing.Api.Services;
using SmartTicketing.Api.Utils;
using SmartTicketing.Application.Interfaces;
using SmartTicketing.Domain;

namespace SmartTicketing.Api.Bootstrap;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiKeyAuthorizationFilter>();
        services.AddSingleton<IApiKeyValidatorService, ApiKeyValidatorService>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: store key in key vault for prod not in appsettings
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration.GetValue<string>("JwtSettings:Issuer"),
                ValidAudience = configuration.GetValue<string>("JwtSettings:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    configuration.GetValue<string>("JwtSettings:Key")!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        // TODO: define policy for admin users so that some endpoints can be accessed only based on that
        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddMyControllers(this IServiceCollection services)
    {
        services.AddHealthChecks();

        // TODO: Add fluent validations
        services.AddControllers(x => x.Filters.Add<ApiKeyAuthorizationFilter>())
            .AddOData(options =>
            {
                options.Select().Filter().OrderBy().SkipToken();
                options.AddRouteComponents("odata", GetEdmModel()); // needed to apply the 'select' odata - calling the 'api' prefix route will get the full object regardless of the query params. This will also map int to enum string
            });

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<BootInit>();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Ticketing", Version = "v1" });
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "The API Key to access the API",
                Name = "X-API-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
            {
                { scheme, new List<string>() }
            };

            c.AddSecurityRequirement(requirement);
        });

        return services;
    }

    public static IServiceCollection AddSignalRConfig(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Ticket>("Tickets");
        return builder.GetEdmModel();
    }
}
