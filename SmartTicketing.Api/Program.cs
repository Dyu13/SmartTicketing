using SmartTicketing.Api.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .AddServices()
    .Build();

app.Configure();

await app.RunAsync();