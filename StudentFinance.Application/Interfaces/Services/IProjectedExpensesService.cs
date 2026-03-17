namespace StudentFinance.Application.Interfaces.Services
{
    public interface IProjectedExpensesService
    {
        Task<decimal> CalculateNextMonthProjectionAsync(Guid familyId, CancellationToken cancellationToken);
    }
}
