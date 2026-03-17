using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } // Student or Parent: Consistency with the Domain
        public Guid? FamilyId { get; set; }
    }
}
