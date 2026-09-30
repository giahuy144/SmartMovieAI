namespace AuthService.Models;

public class User
{
    public int IdUser { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
}
