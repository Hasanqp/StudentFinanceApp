using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentFinance.Application.DTOs.Auth;
using StudentFinance.Application.Interfaces.Services;
using StudentFinance.Application.Settings;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Enums;
using StudentFinance.Domain.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentFinance.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtOptions, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: email already exists {Email}", email);
                return new AuthResponse { ErrorMessage = "Email is already in use." };
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.Student
            };

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user);
            var expiration = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            _logger.LogInformation("User registered successfully: {Email}", email);

            return new AuthResponse
            {
                Token = token,
                TokenType = "Bearer",
                Expiration = expiration,
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role
            };
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found {Email}", email);
                return null;
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed: wrong password {Email}", email);
                return null;
            }

            var token = GenerateJwtToken(user);
            var expiration = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            _logger.LogInformation("User logged in successfully: {Email}", email);

            return new AuthResponse
            {
                Token = token,
                TokenType = "Bearer",
                Expiration = expiration,
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role,
                FamilyId = user.FamilyId
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            if (user.FamilyId.HasValue)
            {
                claims.Add(new Claim("FamilyId", user.FamilyId.Value.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
