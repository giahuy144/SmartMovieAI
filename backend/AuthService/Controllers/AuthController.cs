using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private static readonly List<UserModel> Users = new()
        {
            new UserModel { Id = 1, Name = "Phan Nguyễn Gia Huy", Email = "admin@smartmovie.ai", Role = "ADMIN" },
            new UserModel { Id = 2, Name = "Demo User", Email = "user@gmail.com", Role = "USER" }
        };

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and Password are required.");

            var newUser = new UserModel
            {
                Id = Users.Count + 1,
                Name = dto.Name,
                Email = dto.Email,
                Role = "USER"
            };
            Users.Add(newUser);

            return Ok(new
            {
                message = "Đăng ký tài khoản thành công!",
                user = newUser
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = Users.FirstOrDefault(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                user = new UserModel
                {
                    Id = 100,
                    Name = dto.Email.Split('@')[0],
                    Email = dto.Email,
                    Role = dto.Email.Contains("admin") ? "ADMIN" : "USER"
                };
            }

            string dummyJwtToken = $"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ7dXNlci5JZH0iLCJuYW1lIjoie3VzZXIuTmFtZX0iLCJyb2xlIjoie3VzZXIuUm9sZX0iLCJpYXQiOjE1MTYyMzkwMjJ9.mock_token_{Guid.NewGuid()}";

            return Ok(new
            {
                token = dummyJwtToken,
                user = user,
                expiresIn = 86400
            });
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshDto dto)
        {
            return Ok(new
            {
                token = $"refreshed_token_{Guid.NewGuid()}",
                expiresIn = 86400
            });
        }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "USER";
    }

    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
