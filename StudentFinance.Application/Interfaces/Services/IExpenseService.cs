using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<Expense> CreateExpenseAsync(Guid studentId, decimal localAmount, Currency localCurrency, ExpenseCategory category, string note, CancellationToken cancellationToken);
    }
}
