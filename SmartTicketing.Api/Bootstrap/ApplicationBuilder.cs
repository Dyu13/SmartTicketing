using SmartTicketing.Application;
using SmartTicketing.Infrastructure;
using SmartTicketing.Infrastructure.RealTimeCommunication;

namespace SmartTicketing.Api.Bootstrap;

public static class ApplicationBuilder
{
    // TODO: AddLogging

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        string MyAllowSpecificOrigins = "CorsPolicy"; // TODO: allow only specific origin
        builder.Services.AddCors(options =>
                        options.AddPolicy(MyAllowSpecificOrigins,
                            builder =>
                            {
                                builder
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .SetIsOriginAllowed((host) => true) // AllowAnyOrigin cannot be used with AllowCredentials, so the Origin is set
                                    .AllowCredentials(); //needed for SignalR Cors
                            }));

        // Add services to the container.
        builder.Services
            .AddInfrastructureServices() // TODO: pass builder.Configuration
            .AddApplicationServices()
            .AddApiServices()
            .AddAuth(builder.Configuration)
            .AddMyControllers()
            .AddHostedServices()
            .AddSwagger()
            .AddSignalRConfig();

        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("CorsPolicy");

        app.MapHealthChecks("/healthy");

        app.MapControllers();

        app.MapHub<SummaryHub>("/summaryHub"); // TODO: use authorization for the hub as well

        return app;
    }
}
