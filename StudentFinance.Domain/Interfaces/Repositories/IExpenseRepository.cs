using StudentFinance.Domain.Entities;

namespace StudentFinance.Domain.Interfaces.Repositories
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        // Get a specific family's expenses (for transparency)
        Task<IEnumerable<Expense>> GetExpensesByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken);

        // Monthly expenses for a student
        Task<IEnumerable<Expense>> GetStudentMonthlyExpensesAsync(Guid studentId, int month, int year, CancellationToken cancellationToken);
    }
}
