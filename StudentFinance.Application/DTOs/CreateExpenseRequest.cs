using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.DTOs
{
    public class CreateExpenseRequest
    {
        public decimal LocalAmount { get; set; } // must be > 0
        public Currency LocalCurrency { get; set; }
        public ExpenseCategory Category { get; set; }
        public string Note { get; set; } = string.Empty;
        // Note: The StudentId is usually extracted from the JWT Token in the API
        // but it can be placed here if needed.
    }
}
