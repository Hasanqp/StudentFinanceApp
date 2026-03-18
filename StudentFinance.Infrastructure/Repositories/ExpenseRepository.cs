using Microsoft.EntityFrameworkCore;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Interfaces.Repositories;
using StudentFinance.Infrastructure.Data;

namespace StudentFinance.Infrastructure.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken)
        {
            // Bring all family expenses sorted from newest to oldest
            return await _dbSet
                .AsNoTracking()
                .Where(e => e.FamilyId == familyId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetStudentMonthlyExpensesAsync(Guid studentId, int month, int year, CancellationToken cancellationToken)
        {
            // Bring the student's expenses for a specific month and year
            return await _dbSet
                .AsNoTracking()
                .Where(e => e.UserId == studentId &&
                e.Date.Month == month &&
                e.Date.Year == year)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }
    }
}
