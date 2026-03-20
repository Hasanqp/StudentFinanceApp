using StudentFinance.Application.Interfaces.Services;
using StudentFinance.Domain.Interfaces.Repositories;

namespace StudentFinance.Application.Services
{
    public class ProjectedExpensesService : IProjectedExpensesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectedExpensesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<decimal> CalculateNextMonthProjectionAsync(Guid familyId, CancellationToken cancellationToken)
        {
            // 1. Bring the commitments that are due next month
            var nextMonthDate = DateTime.UtcNow.AddMonths(1);
            var upcomingObligations = await _unitOfWork.Obligations
                .GetUpcomingObligationsAsync(familyId, nextMonthDate, cancellationToken);

            // Calculate total liabilities in family currency
            decimal projectedObligations = upcomingObligations
                .Where(o => !o.IsPaid)
                .Sum(o => o.FamilyAmount);

            // 2. Obtain an effective financial plan (if available) to estimate fixed monthly expenses
            // (This logic can be extended later to calculate the average of the last 3 months instead of the plan)
            decimal expectedMonthlyAllowance = 0; // TODO: Replace with actual financial planning logic

            // 3. Returning the expected cost (liabilities + expected expense)
            return projectedObligations + expectedMonthlyAllowance;
        }
    }
}
