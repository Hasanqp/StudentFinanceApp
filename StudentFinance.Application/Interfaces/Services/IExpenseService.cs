using StudentFinance.Application.DTOs;
using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<ExpenseResponse> CreateExpenseAsync(Guid studentId, decimal localAmount, Currency localCurrency, ExpenseCategory category, string note, CancellationToken cancellationToken);

        Task<IEnumerable<ExpenseResponse>> GetStudentMonthlyExpensesAsync(Guid studentId, int month, int year, CancellationToken cancellationToken);
    }
}
