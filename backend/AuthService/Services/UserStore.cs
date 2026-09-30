using AuthService.Models;

namespace AuthService.Services;

public interface IUserStore
{
    User? FindByUserName(string userName);
    User Create(string userName, string passwordHash);
}

public class UserStore : IUserStore
{
    private readonly object _lock = new();
    private readonly List<User> _users;

    public UserStore(IPasswordHasher passwordHasher)
    {
        _users =
        [
            new User { IdUser = 1, UserName = "use1", PasswordHash = passwordHasher.Hash("123") },
            new User { IdUser = 2, UserName = "user2", PasswordHash = passwordHasher.Hash("123") }
        ];
    }

    public User? FindByUserName(string userName)
    {
        lock (_lock)
            return _users.FirstOrDefault(user => user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
    }

    public User Create(string userName, string passwordHash)
    {
        lock (_lock)
        {
            var user = new User { IdUser = _users.Count + 1, UserName = userName, PasswordHash = passwordHash };
            _users.Add(user);
            return user;
        }
    }
}
