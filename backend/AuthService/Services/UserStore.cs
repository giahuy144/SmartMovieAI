using AuthService.Data;
using AuthService.Models;

namespace AuthService.Services;

public interface IUserStore
{
    User? FindByUserName(string userName);
    User Create(string userName, string passwordHash);
}

public class SqlUserStore : IUserStore
{
    private readonly AuthDbContext _db;

    public SqlUserStore(AuthDbContext db) => _db = db;

    public User? FindByUserName(string userName) =>
        _db.Users.FirstOrDefault(user => user.UserName == userName);

    public User Create(string userName, string passwordHash)
    {
        var user = new User
        {
            UserName = userName,
            Email = $"{userName.Trim().ToLowerInvariant()}@smartmovie.local",
            PasswordHash = passwordHash,
            Role = "User"
        };

        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }
}
