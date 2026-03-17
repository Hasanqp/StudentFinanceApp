using StudentFinance.Application.Exceptions;
using StudentFinance.Application.Interfaces.Services;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Enums;
using StudentFinance.Domain.Interfaces.Repositories;

namespace StudentFinance.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyExchangeService _currencyService;
        
        public ExpenseService(IUnitOfWork unitOfWork, ICurrencyExchangeService currencyService)
        {
            _unitOfWork = unitOfWork;
            _currencyService = currencyService;
        }

        public async Task<Expense> CreateExpenseAsync(Guid studentId, decimal localAmount, Currency localCurrency, ExpenseCategory category, string note, CancellationToken cancellationToken)
        {
            // 1. Retrieve the student's data to identify his family
            var student = await _unitOfWork.Users.GetByIdAsync(studentId, cancellationToken);
            if (student == null)
                throw new NotFoundException("Student not found");

            if (student.FamilyId == null)
                throw new NotFoundException("Student is not assigned to a family");

            // 2. Retrieve family data to determine the base currency
            var family = await _unitOfWork.Families.GetByIdAsync(
                student.FamilyId.Value, cancellationToken);
            if (family == null)
                throw new NotFoundException("Family not found");

            // 3. Get the exchange rate
            var exchangeRate = await _currencyService.GetExchangeRateAsync(
                localCurrency, family.BaseCurrency, cancellationToken);

            // 4. Creating the Entity
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                UserId = studentId,
                FamilyId = family.Id,
                Category = category,
                Note = note,
                Date = DateTime.UtcNow,
                LocalAmount = localAmount,
                LocalCurrency = localCurrency,
                FamilyAmount = localAmount * exchangeRate, // Amount in the family's base currency
                FamilyCurrency = family.BaseCurrency,
                AppliedExchangeRate = exchangeRate
            };

            // 5. Saving to the database via UnitOfWork
            await _unitOfWork.Expenses.AddAsync(expense, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return expense;
        }
    }
}
