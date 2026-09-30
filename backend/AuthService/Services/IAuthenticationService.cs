using AuthService.Contracts;

namespace AuthService.Services;

public interface IAuthenticationService
{
    AuthenticationResult Login(LoginRequest request);
    AuthenticationResult Register(RegisterRequest request);
}

public record AuthenticationResult(bool Success, AuthResponse? Response, string? Error)
{
    public static AuthenticationResult Failed(string error) => new(false, null, error);
    public static AuthenticationResult Succeeded(AuthResponse response) => new(true, response, null);
}
