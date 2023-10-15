using System.Text;

using RabbitMQ.Client.Events;
using RabbitMQ.Client;

using SmartTicketing.Common.RabbitMq;

namespace SmartTicketing.Ai.RabbitMq;

public class Subscriber : ISubscriber
{
    private readonly IConnectionProvider _connectionProvider;
    private readonly string _exchange;
    private readonly string _queue;
    private readonly IModel _model;
    private bool _disposed;

    public Subscriber(
        IConnectionProvider connectionProvider,
        string exchange,
        string queue,
        string routingKey,
        string exchangeType,
        int timeToLive = 30000,
        ushort prefetchSize = 10)
    {
        _connectionProvider = connectionProvider;
        _exchange = exchange;
        _queue = queue;

        var connection = _connectionProvider.GetConnection();
        if (connection == null) return; // TODO: log when unable to establish a connection

        _model = _connectionProvider.GetConnection().CreateModel();
        if(_model == null) return;

        var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };
        _model.ExchangeDeclare(_exchange, exchangeType, arguments: ttl);
        _model.QueueDeclare(_queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _model.QueueBind(_queue, _exchange, routingKey);
        _model.BasicQos(0, prefetchSize, false);
    }

    public void Subscribe(Func<string, IDictionary<string, object>, bool> callback)
    {
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            bool success = callback.Invoke(message, e.BasicProperties.Headers);
            if (success)
            {
                _model.BasicAck(e.DeliveryTag, true);
            }
        };

        if (_model == null) return;

        _model.BasicConsume(_queue, false, consumer);
    }

    public async Task SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
    {
        var consumer = new AsyncEventingBasicConsumer(_model);
        consumer.Received += async (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            bool success = await callback.Invoke(message, e.BasicProperties.Headers);
            if (success)
            {
                _model.BasicAck(e.DeliveryTag, true);
            }
        };

        if (_model == null) return;

        _model.BasicConsume(_queue, false, consumer);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _model?.Close();

        _disposed = true;
    }
}
