using StudentFinance.Domain.Entities;

namespace StudentFinance.Domain.Interfaces.Repositories
{
    public interface IObligationRepository : IGenericRepository<Obligation>
    {
        // Get all obligations for a specific family
        Task<IEnumerable<Obligation>> GetObligationsByFamilyIdAsync(Guid familyId);

        // Get all unpaid obligations for a specific family 
        Task<IEnumerable<Obligation>> GetUnpaidObligationsAsync(Guid familyId);

        // Get obligations that are due within the next month for a specific family
        Task<IEnumerable<Obligation>> GetUpcomingObligationsAsync(Guid familyId, DateTime beforeDate);
    }
}
