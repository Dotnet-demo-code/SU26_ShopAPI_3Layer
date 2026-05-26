using BusinessLayer.IServices;
using DataAccessLayer.DTOs.User;
using DataAccessLayer.IRepository;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecretKey;
        private readonly int _jwtExpirationMinutes;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");
            _jwtExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone
            }).ToList();
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        public async Task<UserDTO> CreateAsync(UserRegisterDTO userRegisterDTO)
        {
            // Kiểm tra username đã tồn tại
            var existingUser = await _userRepository.GetByUsernameAsync(userRegisterDTO.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username đã tồn tại");

            var user = new User
            {
                Username = userRegisterDTO.Username,
                PasswordHash = HashPassword(userRegisterDTO.Password),
                FullName = userRegisterDTO.FullName,
                Email = userRegisterDTO.Email,
                Phone = userRegisterDTO.Phone
            };

            var createdUser = await _userRepository.AddNewAsync(user);

            return new UserDTO
            {
                UserId = createdUser.UserId,
                Username = createdUser.Username,
                FullName = createdUser.FullName,
                Email = createdUser.Email,
                Phone = createdUser.Phone
            };
        }

        public async Task<UserDTO> UpdateAsync(UserDTO userUpdateDTO)
        {
            var user = await _userRepository.GetByIdAsync(userUpdateDTO.UserId);
            if (user == null)
                throw new KeyNotFoundException("User không tồn tại");

            user.FullName = userUpdateDTO.FullName;
            user.Email = userUpdateDTO.Email;
            user.Phone = userUpdateDTO.Phone;

            var updatedUser = await _userRepository.UpdateAsync(user);

            return new UserDTO
            {
                UserId = updatedUser.UserId,
                Username = updatedUser.Username,
                FullName = updatedUser.FullName,
                Email = updatedUser.Email,
                Phone = updatedUser.Phone
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.DeleteByIdAsync(id);
            return user != null;
        }

        public async Task<LoginResponseDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(userLoginDTO.Username);

            if (user == null || !VerifyPassword(userLoginDTO.Password, user.PasswordHash))
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Username hoặc password không đúng"
                };
            }

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Token = token,
                User = new UserDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone
                }
            };
        }

        public async Task<UserDTO> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            return await CreateAsync(userRegisterDTO);
        }

        // Helper Methods
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim("UserId", user.UserId.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Username),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "ShopAPI",
                audience: "ShopAPIUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }
}
