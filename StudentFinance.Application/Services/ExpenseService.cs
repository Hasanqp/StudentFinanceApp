using Microsoft.Extensions.Logging;
using StudentFinance.Application.DTOs;
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
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(IUnitOfWork unitOfWork, ICurrencyExchangeService currencyService, ILogger<ExpenseService> logger)
        {
            _unitOfWork = unitOfWork;
            _currencyService = currencyService;
            _logger = logger;
        }

        public async Task<ExpenseResponse> CreateExpenseAsync(Guid studentId, decimal localAmount, Currency localCurrency, ExpenseCategory category, string note, CancellationToken cancellationToken)
        {
            // 1. Retrieve student
            var student = await _unitOfWork.Users.GetByIdAsync(studentId, cancellationToken);

            if (student == null)
            {
                _logger.LogWarning("Student not found: {StudentId}", studentId);
                throw new NotFoundException("Student not found");
            }

            if (student.FamilyId == null)
                throw new NotFoundException("Student is not assigned to a family");

            // 2. Retrieve family
            var family = await _unitOfWork.Families.GetByIdAsync(
                student.FamilyId.Value, cancellationToken);

            if (family == null)
                throw new NotFoundException("Family not found");

            // 3. Get exchange rate
            var appliedExchangeRate = await _currencyService.GetExchangeRateAsync(
                localCurrency, family.BaseCurrency, cancellationToken);
            
            // 4. Create entity
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
                FamilyAmount = localAmount * appliedExchangeRate, // Amount in the family's base currency
                FamilyCurrency = family.BaseCurrency,
                AppliedExchangeRate = appliedExchangeRate
            };

            // 5. Saving to the database via UnitOfWork
            await _unitOfWork.Expenses.AddAsync(expense, cancellationToken);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Expense created: UserId={UserId}, Amount={Amount}, Currency={Currency}",
                studentId,
                localAmount,
                localCurrency
            );

            // 6. Map to DTO
            return new ExpenseResponse
            {
                Id = expense.Id,
                LocalAmount = expense.LocalAmount,
                LocalCurrency = expense.LocalCurrency,
                FamilyAmount = expense.FamilyAmount,
                FamilyCurrency = expense.FamilyCurrency,
                AppliedExchangeRate = expense.AppliedExchangeRate,
                Category = expense.Category,
                Note = expense.Note,
                Date = expense.Date
            };
        }

        public async Task<IEnumerable<ExpenseResponse>> GetStudentMonthlyExpensesAsync(Guid studentId, int month, int year, CancellationToken cancellationToken)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12");

            if (year < 2000 || year > DateTime.UtcNow.Year + 1)
                throw new ArgumentException("Invalid year");

            // 1. Validate student exists
            var student = await _unitOfWork.Users.GetByIdAsync(studentId, cancellationToken);

            if (student == null)
            {
                _logger.LogWarning("Student not found: {StudentId}", studentId);
                throw new NotFoundException("Student not found");
            }

            // 2. Get expenses from repository
            var expenses = await _unitOfWork.Expenses
                .GetStudentMonthlyExpensesAsync(studentId, month, year, cancellationToken);

            // 3. Map domain entities to response DTOs
            return expenses.Select(expense => new ExpenseResponse
            {
                Id = expense.Id,
                LocalAmount = expense.LocalAmount,
                LocalCurrency = expense.LocalCurrency,
                FamilyAmount = expense.FamilyAmount,
                FamilyCurrency = expense.FamilyCurrency,
                AppliedExchangeRate = expense.AppliedExchangeRate,
                Category = expense.Category,
                Note = expense.Note,
                Date = expense.Date
            }).ToList();
        }
    }
}
