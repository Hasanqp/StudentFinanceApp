using StudentFinance.Application.DTOs.Family;

namespace StudentFinance.Application.Interfaces.Services
{
    public interface IFamilyService
    {
        // Retrieves a summary of the family's financial status, including total income, total expenses, and net balance.
        Task<FamilySummaryResponse?> GetFamilySummaryAsync(Guid familyId, CancellationToken cancellationToken);
    }
}
