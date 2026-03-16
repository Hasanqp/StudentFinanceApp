using StudentFinance.Domain.Entities;

namespace StudentFinance.Domain.Interfaces.Repositories
{
    public interface IFinancialPlanRepository : IGenericRepository<FinancialPlan>
    {
        // Get all financial plans for a specific student
        Task<IEnumerable<FinancialPlan>> GetPlansByStudentIdAsync(Guid studentId);

        // Get the active financial plan for a student (the one that is currently in effect based on the current date)
        Task<FinancialPlan?> GetActivePlanAsync(Guid studentId, DateTime currentDate);
    }
}
