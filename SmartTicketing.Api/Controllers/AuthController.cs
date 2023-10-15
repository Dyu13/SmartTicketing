using Microsoft.AspNetCore.Mvc;

using SmartTicketing.Api.Models;
using SmartTicketing.Application.Auth;

namespace SmartTicketing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger _logger;

    public AuthController(
        IAuthService authService,
        ILogger logger)
    {
        _authService = authService;
        _logger = logger;

    }

    [HttpPost]
    public async Task<ActionResult<string>> Login([FromBody] AuthModel authModel)
    {
        try
        {
            var token = await _authService.AuthenticateAsync(authModel.User, authModel.Password);
            return Ok(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
