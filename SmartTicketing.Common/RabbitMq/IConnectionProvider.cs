using RabbitMQ.Client;

namespace SmartTicketing.Common.RabbitMq;

public interface IConnectionProvider : IDisposable
{
    IConnection GetConnection();
}
