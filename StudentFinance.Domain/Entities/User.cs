using StudentFinance.Domain.Enums;

namespace StudentFinance.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Guid? FamilyId { get; set; } // Connecting with family
        public Family? Family { get; set; }
        public UserRole Role { get; set; } // Student or Parent
    }
}
