using StudentFinance.Domain.Enums;

namespace StudentFinance.Domain.Entities
{
    public class Family
    {
        public Guid Id { get; set; }
        public string FamilyName { get; set; } = string.Empty;

        // Optional country field for potential localization features
        public Country Country { get; set; }

        // Base currency for the family
        public string BaseCurrency { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<User> Members { get; set; } = new List<User>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Obligation> Obligations { get; set; } = new List<Obligation>();
    }
}
