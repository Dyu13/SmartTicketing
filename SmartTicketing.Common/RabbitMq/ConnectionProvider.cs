using RabbitMQ.Client;

namespace SmartTicketing.Common.RabbitMq;

public class ConnectionProvider : IConnectionProvider
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private bool _disposed;

    public ConnectionProvider(string host)
    {
        Task.Delay(15000).Wait(); // TODO: use Polly or better use docker-compose helthcheck to await the rabbitmq service to be ready (depends-on and wait-for-it.sh were not enough)
        _factory = new ConnectionFactory
        {
            HostName = host,
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        try
        {
            _connection = _factory.CreateConnection();
        }
        catch (Exception)
        {
            // TODO: log
        }
    }

    public IConnection GetConnection()
    {
        return _connection;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _connection?.Close();

        _disposed = true;
    }
}
