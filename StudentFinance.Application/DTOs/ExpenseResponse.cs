using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.DTOs
{
    public class ExpenseResponse
    {
        public Guid Id { get; set; }
        public decimal LocalAmount { get; set; }
        public Currency LocalCurrency { get; set; }

        public decimal FamilyAmount { get; set; }
        public Currency FamilyCurrency { get; set; }
        public decimal AppliedExchangeRate { get; set; }
        public ExpenseCategory Category { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
