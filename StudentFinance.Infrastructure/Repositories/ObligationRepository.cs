using Microsoft.EntityFrameworkCore;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Interfaces.Repositories;
using StudentFinance.Infrastructure.Data;

namespace StudentFinance.Infrastructure.Repositories
{
    public class ObligationRepository : GenericRepository<Obligation>, IObligationRepository
    {
        public ObligationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Obligation>> GetObligationsByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().Where(o => o.FamilyId == familyId)
                                      .OrderBy(o => o.DueDate)
                                      .ToListAsync();
        }

        public async Task<IEnumerable<Obligation>> GetUnpaidObligationsAsync(Guid familyId, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().Where(o => o.FamilyId == familyId && !o.IsPaid)
                                      .OrderBy(o => o.DueDate)
                                      .ToListAsync();
        }

        public async Task<IEnumerable<Obligation>> GetUpcomingObligationsAsync(Guid familyId, DateTime beforeDate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().Where(o => o.FamilyId == familyId &&
                                                  !o.IsPaid &&
                                                  o.DueDate <= beforeDate)
                                      .OrderBy(o => o.DueDate)
                                      .ToListAsync();
        }
    }
}
