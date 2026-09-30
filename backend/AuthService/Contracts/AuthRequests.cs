namespace AuthService.Contracts;

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest : LoginRequest
{
    public string ConfirmPassword { get; set; } = string.Empty;
}
