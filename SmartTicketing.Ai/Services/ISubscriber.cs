namespace SmartTicketing.Ai.RabbitMq;

public interface ISubscriber : IDisposable
{
    void Subscribe(Func<string, IDictionary<string, object>, bool> callback);
    Task SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback);
}
