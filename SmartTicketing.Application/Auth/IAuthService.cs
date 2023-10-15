namespace SmartTicketing.Application.Auth;

public interface IAuthService
{
    Task<string> AuthenticateAsync(string username, string password);
}
