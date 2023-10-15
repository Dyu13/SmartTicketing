namespace SmartTicketing.Application.Interfaces;

public interface IApiKeyValidatorService
{
    bool IsValid(string apiKey);
}
