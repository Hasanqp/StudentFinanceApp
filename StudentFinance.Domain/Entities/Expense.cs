using StudentFinance.Domain.Enums;

namespace StudentFinance.Domain.Entities
{
    public class Expense
    {
        public Guid Id { get; set; }
        public ExpenseCategory Category { get; set; } // type of expense
        public string Note { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // Local amount and parent
        public decimal LocalAmount { get; set; }
        public Currency LocalCurrency { get; set; }

        // Amount in a common family currency
        public decimal FamilyAmount { get; set; }
        public Currency FamilyCurrency { get; set; }
        public decimal AppliedExchangeRate { get; set; } // Exchange rate used for converting local amount to family amount
        public Guid UserId { get; set; }
        public Guid FamilyId { get; set; }
        public User? User { get; set; }
        public Family? Family { get; set; }
    }
}
