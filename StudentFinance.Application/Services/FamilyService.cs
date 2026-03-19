using StudentFinance.Application.DTOs.Family;
using StudentFinance.Application.Exceptions;
using StudentFinance.Application.Interfaces.Services;
using StudentFinance.Domain.Interfaces.Repositories;

namespace StudentFinance.Application.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FamilyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FamilySummaryResponse?> GetFamilySummaryAsync(Guid familyId, CancellationToken cancellationToken)
        {
            // 1. Get family with members
            var family = await _unitOfWork.Families.GetFamilyWithMembersAsync(familyId, cancellationToken);

            if (family == null)
                return null;

            // 2. Calculate current month expenses
            var now = DateTime.UtcNow;
            var allExpenses = await _unitOfWork.Expenses.GetExpensesByFamilyIdAsync(familyId, cancellationToken);

            decimal monthlyExpenses = allExpenses
                .Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
                .Sum(e => e.FamilyAmount);

            // 3. Calculate total unpaid obligations
            var unpaidObligations = await _unitOfWork.Obligations.GetUnpaidObligationsAsync(familyId, cancellationToken);
            decimal totalUnpaid = unpaidObligations.Sum(o => o.FamilyAmount);

            // 4. Map to DTO
            return new FamilySummaryResponse
            {
                FamilyId = family.Id,
                FamilyName = family.FamilyName,
                BaseCurrency = family.BaseCurrency, // Use the proper Enum from your domain
                TotalMonthlyExpenses = monthlyExpenses,
                TotalUnpaidObligations = totalUnpaid,
                MembersCount = family.Members.Count
            };
        }
    }
}
