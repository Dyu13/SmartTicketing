using Microsoft.AspNetCore.Mvc;

namespace SmartTicketing.Api.Utils;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
: base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
