using System.Text.Json;
using SmartTicketing.Ai.RabbitMq;
using SmartTicketing.Ai.Services;
using SmartTicketing.Common;

namespace SmartTicketing.Ai;

public class App
{
    private readonly ISubscriber _subscriber;
    private readonly ISignalRClient _signalRClient;

    public App(
        ISubscriber subscriber,
        ISignalRClient signalRClient)
    {
        _subscriber = subscriber;
        _signalRClient = signalRClient;
    }

    public async void Run(string[] args)
    {
        // _logger.LogInformation("Start Listening");
        Console.WriteLine("Start Listening");
        _subscriber.Subscribe((message, header) =>
        {
            try
            {
                var payload = JsonSerializer.Deserialize<MessagePayload>(message);

                // _logger.LogInformation($"New message: {payload.Message}");
                Console.WriteLine($"New message: {payload.Message}");

                payload.Message = payload.Message.Substring(0, 25);

                // _logger.LogInformation($"Summary: {payload.Message}");
                Console.WriteLine($"Summary: {payload.Message}");

                message = JsonSerializer.Serialize(payload);

                _signalRClient.SendMessage(message);

                return true;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message, new { App = "AI", Method = "Compute Ticket Description Handler" });
                return false;
            }
        });
    }
}
