using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using RabbitMQ.Client;

using SmartTicketing.Application.Interfaces;
using SmartTicketing.Common;
using SmartTicketing.Common.RabbitMq;
using SmartTicketing.Domain;
using SmartTicketing.Infrastructure.Persistence;
using SmartTicketing.Infrastructure.RabbitMq;

namespace SmartTicketing.Infrastructure;

public static class ConfigureServices
{
    // TODO: inject IConfiguration when using a real SQL database to pass the connection string from the API
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<SmartTicketingDbContext>(options => options.UseInMemoryDatabase("SmartTicketingDb"));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SmartTicketingDbContext>());

        services.AddSingleton<IConnectionProvider>(new ConnectionProvider("rabbitmq"));
        services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
            AppConstants.AmqpExchange,
            ExchangeType.Topic));

        var config = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Ticket, Ticket>(); // TODO: when the app grows, make use of a DTO
        });
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}