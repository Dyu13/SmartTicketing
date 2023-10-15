namespace SmartTicketing.Ai.Services;

public interface ISignalRClient
{
    public Task SendMessage(string message);
}
