namespace AuthService.Contracts;

public record AuthResponse(string Message, string Token, UserResponse User);
public record UserResponse(int IdUser, string UserName);
