using Microsoft.AspNetCore.SignalR.Client;

namespace SmartTicketing.Ai.Services;

public class SignalRClient : ISignalRClient
{
    public async Task SendMessage(string message)
    {
        var connection = new HubConnectionBuilder()
                .WithUrl("http://smart-ticketing-api:13600/summaryHub")
                .Build();

        //connection.Closed += async (error) =>
        //{
        //    await Task.Delay(new Random().Next(0, 5) * 1000);
        //    await connection.StartAsync();
        //};

        try
        {
            await connection.StartAsync();

            await connection.InvokeAsync("SendSummary", message);

            await connection.StopAsync(); // not really needed after the result is sent, a new one will be created for a different ticket
        }
        catch (Exception ex)
        {
            // TODO: log
        }
    }
}
