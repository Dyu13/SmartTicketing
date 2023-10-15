using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartTicketing.Application.Interfaces;

namespace SmartTicketing.Api.Utils;

public class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";

    private readonly IApiKeyValidatorService _apiKeyValidator;

    public ApiKeyAuthorizationFilter(IApiKeyValidatorService apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key missing");
            return;
        }

        if (!_apiKeyValidator.IsValid(apiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        }
    }
}
