using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.DTOs.Family
{
    public class FamilySummaryResponse
    {
        public Guid FamilyId { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public Currency BaseCurrency { get; set; } // Family currency (such as SAR)

        // Quick financial summary
        public decimal TotalMonthlyExpenses { get; set; } // Total expenses for the current month
        public decimal TotalUnpaidObligations { get; set; } // Total outstanding liabilities
        public int MembersCount { get; set; } // Number of family members
    }
}
