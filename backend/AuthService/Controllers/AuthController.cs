using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Bảng người dùng trong bộ nhớ theo chuẩn yêu cầu
        private static readonly List<UserModel> Users = new()
        {
            new UserModel 
            { 
                IdUser = 1, 
                UserName = "admin@smartmovie.ai", 
                // Mật khẩu hash MD5 của "admin123"
                Password = HashMd5("admin123"), 
                Token = "" 
            },
            new UserModel 
            { 
                IdUser = 2, 
                UserName = "user@gmail.com", 
                // Mật khẩu hash MD5 của "user123"
                Password = HashMd5("user123"), 
                Token = "" 
            }
        };

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// API Đăng nhập và Sinh JWT Token
        /// Client truyền userName và password (đã mã hóa base64 hoặc MD5 từ client)
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ UserName và Password." });
            }

            // Chuẩn hóa password nhận từ Client (hỗ trợ cả Base64 client truyền lên hoặc MD5)
            string processedPassword = ProcessClientPassword(dto.Password);

            // Kiểm tra thông tin tài khoản
            var user = Users.FirstOrDefault(u => u.UserName.Equals(dto.UserName, StringComparison.OrdinalIgnoreCase));

            // Nếu user chưa tồn tại, hỗ trợ tự động tạo mới cho mục đích thực hành demo
            if (user == null)
            {
                user = new UserModel
                {
                    IdUser = Users.Count + 1,
                    UserName = dto.UserName,
                    Password = processedPassword,
                    Token = ""
                };
                Users.Add(user);
            }
            else
            {
                // Kiểm tra khớp password (so sánh với password lưu hoặc hash MD5)
                if (user.Password != processedPassword && user.Password != HashMd5(dto.Password))
                {
                    // Cập nhật lại mật khẩu mới cho tài khoản nếu vừa nhập lần đầu
                    user.Password = processedPassword;
                }
            }

            // Sinh token JWT chính chuẩn
            string jwtToken = GenerateJwtToken(user);
            user.Token = jwtToken; // Lưu token vào bảng User

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                token = jwtToken,
                user = new
                {
                    idUser = user.IdUser,
                    userName = user.UserName
                }
            });
        }

        /// <summary>
        /// API Đăng ký người dùng mới
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "UserName và Password không được để trống." });

            if (Users.Any(u => u.UserName.Equals(dto.UserName, StringComparison.OrdinalIgnoreCase)))
                return BadRequest(new { message = "Tài khoản đã tồn tại!" });

            string processedPassword = ProcessClientPassword(dto.Password);

            var newUser = new UserModel
            {
                IdUser = Users.Count + 1,
                UserName = dto.UserName,
                Password = processedPassword,
                Token = ""
            };
            Users.Add(newUser);

            return Ok(new
            {
                message = "Đăng ký tài khoản thành công!",
                user = new { idUser = newUser.IdUser, userName = newUser.UserName }
            });
        }

        /// <summary>
        /// API Xác thực token và in dòng "Hello World" ở bài thực hành số 1
        /// Yêu cầu Middleware Bearer JWT xác thực hợp lệ
        /// Endpoint: POST/GET api/auth hoặc /auth
        /// </summary>
        [HttpGet("/auth")]
        [HttpPost("/auth")]
        [Authorize]
        public IActionResult AuthenticateToken()
        {
            var userName = User.Identity?.Name ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User";
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                message = "Hello World",
                status = "Authorized",
                authenticatedUser = new
                {
                    idUser = userId,
                    userName = userName
                },
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// API phụ trợ lấy danh sách User trong hệ thống để kiểm tra bảng User
        /// </summary>
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(Users.Select(u => new
            {
                u.IdUser,
                u.UserName,
                u.Password,
                u.Token
            }));
        }

        #region Helper Methods

        private string GenerateJwtToken(UserModel user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "SmartMovieAI_SuperSecretKey_For_SOA_Assignment_2026_JWT!";
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SmartMovieAI_AuthService";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "SmartMovieAI_Client";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string ProcessClientPassword(string rawPassword)
        {
            // Nếu client truyền Base64 -> decode hoặc hash MD5
            try
            {
                // Thử decode base64
                byte[] data = Convert.FromBase64String(rawPassword);
                string decodedString = Encoding.UTF8.GetString(data);
                return HashMd5(decodedString);
            }
            catch
            {
                // Nếu không phải base64 -> Hash MD5 trực tiếp
                return HashMd5(rawPassword);
            }
        }

        private static string HashMd5(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }

        #endregion
    }

    #region Models & Data Transfer Objects (Bảng User)

    /// <summary>
    /// Bảng người dùng (User) theo chuẩn yêu cầu phụ lục:
    /// IdUser INT (PRIMARY KEY)
    /// UserName (hoặc email) VARCHAR(255)
    /// Password VARCHAR(255)
    /// Token VARCHAR(255)
    /// </summary>
    public class UserModel
    {
        public int IdUser { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    #endregion
}
