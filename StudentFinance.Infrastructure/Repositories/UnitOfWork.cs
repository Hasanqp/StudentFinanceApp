using StudentFinance.Domain.Interfaces.Repositories;
using StudentFinance.Infrastructure.Data;

namespace StudentFinance.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IExpenseRepository Expenses { get; private set; }
        public IFamilyRepository Families { get; private set; }
        public IFinancialPlanRepository FinancialPlans { get; private set; }
        public IObligationRepository Obligations { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // Initialize repositories
            Expenses = new ExpenseRepository(_context);
            Families = new FamilyRepository(_context);
            FinancialPlans = new FinancialPlanRepository(_context);
            Obligations = new ObligationRepository(_context);
            Users = new UserRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
