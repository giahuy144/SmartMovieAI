using System.Text;
using AuthService.Contracts;
using AuthService.Models;

namespace AuthService.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserStore _userStore;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUserStore userStore, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userStore = userStore;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public AuthenticationResult Login(LoginRequest request)
    {
        if (!TryReadCredentials(request.UserName, request.Password, out var userName, out var password, out var error))
            return AuthenticationResult.Failed(error!);

        var user = _userStore.FindByUserName(userName!);
        if (user is null || !_passwordHasher.Verify(password!, user.PasswordHash))
            return AuthenticationResult.Failed("Tên đăng nhập hoặc mật khẩu không đúng.");

        return AuthenticationResult.Succeeded(CreateResponse(user, "Đăng nhập thành công!"));
    }

    public AuthenticationResult Register(RegisterRequest request)
    {
        if (!TryReadCredentials(request.UserName, request.Password, out var userName, out var password, out var error))
            return AuthenticationResult.Failed(error!);

        if (!TryDecodePassword(request.ConfirmPassword, out var confirmPassword) || password != confirmPassword)
            return AuthenticationResult.Failed("Mật khẩu xác nhận không khớp.");

        if (_userStore.FindByUserName(userName!) is not null)
            return AuthenticationResult.Failed("Tài khoản đã tồn tại.");

        var user = _userStore.Create(userName!, _passwordHasher.Hash(password!));
        return AuthenticationResult.Succeeded(CreateResponse(user, "Đăng ký tài khoản thành công!"));
    }

    private AuthResponse CreateResponse(User user, string message) =>
        new(message, _tokenService.CreateToken(user), new UserResponse(user.IdUser, user.UserName, user.Role));

    private static bool TryReadCredentials(string userName, string encodedPassword, out string? normalizedUserName, out string? password, out string? error)
    {
        normalizedUserName = userName?.Trim();
        password = null;
        error = null;

        if (string.IsNullOrWhiteSpace(normalizedUserName) || string.IsNullOrWhiteSpace(encodedPassword))
        {
            error = "Tên đăng nhập và mật khẩu không được để trống.";
            return false;
        }

        if (!TryDecodePassword(encodedPassword, out password) || string.IsNullOrWhiteSpace(password))
        {
            error = "Mật khẩu không hợp lệ.";
            return false;
        }

        return true;
    }

    private static bool TryDecodePassword(string encodedPassword, out string password)
    {
        password = string.Empty;
        try
        {
            password = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
