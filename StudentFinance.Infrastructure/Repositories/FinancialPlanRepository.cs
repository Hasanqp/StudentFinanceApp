using Microsoft.EntityFrameworkCore;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Interfaces.Repositories;
using StudentFinance.Infrastructure.Data;

namespace StudentFinance.Infrastructure.Repositories
{
    public class FinancialPlanRepository : GenericRepository<FinancialPlan>, IFinancialPlanRepository
    {
        public FinancialPlanRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<FinancialPlan?> GetActivePlanAsync(Guid studentId, DateTime currentDate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.StudentId == studentId &&
                                                    p.StartDate <= currentDate &&
                                                    p.EndDate >= currentDate,
                                                    cancellationToken);
        }

        public async Task<IEnumerable<FinancialPlan>> GetPlansByStudentIdAsync(Guid studentId, CancellationToken cancellationToken)
        {
            // Retrieve the plan within whose start and end dates the current date falls
            return await _dbSet.AsNoTracking().Where(p => p.StudentId == studentId)
                                      .OrderByDescending(p => p.StartDate)
                                      .ToListAsync(cancellationToken);
        }
    }
}
