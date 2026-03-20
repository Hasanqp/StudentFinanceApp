using System.ComponentModel.DataAnnotations;

namespace StudentFinance.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$")]
        public string Password { get; set; } = string.Empty;
    }
}
