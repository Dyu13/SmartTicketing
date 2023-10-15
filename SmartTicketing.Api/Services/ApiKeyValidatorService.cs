using SmartTicketing.Application.Interfaces;

namespace SmartTicketing.Api.Services;

public class ApiKeyValidatorService : IApiKeyValidatorService
{
    private readonly string _secret = "SmartTicketingSecret";

    public bool IsValid(string apiKey)
    {
        return apiKey == _secret;
    }
}
