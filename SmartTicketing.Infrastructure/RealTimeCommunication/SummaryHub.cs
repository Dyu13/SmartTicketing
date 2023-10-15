using System.Text.Json;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SmartTicketing.Common;
using SmartTicketing.Common.RabbitMq;
using SmartTicketing.Infrastructure.RabbitMq;

namespace SmartTicketing.Infrastructure.RealTimeCommunication;

// TODO: configure [Authorize]
public class SummaryHub : Hub
{
    private const string Target = "Summary";

    private readonly IPublisher _publisher;
    private readonly ILogger _logger; // TODO: log the messages

    public SummaryHub(
        IPublisher publisher,
        ILogger logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task SendTicketDescription(string ticketDescription)
    {
        var payload = new MessagePayload
        {
            Message = ticketDescription,
            ConnectionId = Context.ConnectionId
        };
        
        var message = JsonSerializer.Serialize(payload);
        _publisher.Publish(message, RoutingKeys.ComputeTicketDescriptionKey, null);
    }

    public async Task SendSummary(string message)
    {
        var payload = JsonSerializer.Deserialize<MessagePayload>(message);

        await Clients.Client(payload.ConnectionId).SendAsync(Target, payload.Message);
    }
}
