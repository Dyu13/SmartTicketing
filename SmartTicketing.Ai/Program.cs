using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RabbitMQ.Client;

using SmartTicketing.Ai;
using SmartTicketing.Common.RabbitMq;
using SmartTicketing.Ai.RabbitMq;
using SmartTicketing.Ai.Services;
using SmartTicketing.Common;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
    // Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

await host.RunAsync();

IHostBuilder CreateHostBuilder(string[] strings)
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<ISignalRClient, SignalRClient>();
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider("rabbitmq"));
            services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
                AppConstants.AmqpExchange,
                AppConstants.QueueName,
                RoutingKeys.ComputeTicketDescriptionKey,
                ExchangeType.Topic));

            services.AddSingleton<App>();
        });
}

