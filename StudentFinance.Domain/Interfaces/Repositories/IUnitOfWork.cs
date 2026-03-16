namespace StudentFinance.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IExpenseRepository Expenses { get; }
        IFamilyRepository Families { get; }
        IFinancialPlanRepository FinancialPlans { get; }
        IObligationRepository Obligations { get; }
        IUserRepository Users { get; }

        Task<int> CompleteAsync(); // Save changes to the database
    }
}
