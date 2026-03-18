using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string? ErrorMessage { get; set; }
        public string Token { get; set; } = string.Empty; // JWT Token
        public string TokenType { get; set; } = "Bearer";
        public DateTimeOffset Expiration { get; set; }
        // Essential information to facilitate the operation of the MAUI application after logging in
        public Guid? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
        public Guid? FamilyId { get; set; }
    }
}
