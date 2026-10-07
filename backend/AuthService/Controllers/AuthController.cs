using AuthService.Contracts;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService) => _authenticationService = authenticationService;

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login([FromBody] LoginRequest request)
    {
        var result = _authenticationService.Login(request);
        return result.Success ? Ok(result.Response) : Unauthorized(new { message = result.Error });
    }

    [HttpPost("register")]
    public ActionResult<AuthResponse> Register([FromBody] RegisterRequest request)
    {
        var result = _authenticationService.Register(request);
        return result.Success ? Ok(result.Response) : BadRequest(new { message = result.Error });
    }

    [HttpGet("/auth")]
    [HttpPost("/auth")]
    [Authorize]
    public IActionResult AuthenticateToken() => Ok(new
    {
        message = "Hello World",
        status = "Authorized",
        authenticatedUser = new
        {
            idUser = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            userName = User.Identity?.Name,
            role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "User"
        },
        timestamp = DateTime.UtcNow
    });
}
