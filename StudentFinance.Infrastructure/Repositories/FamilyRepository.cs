using Microsoft.EntityFrameworkCore;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Interfaces.Repositories;
using StudentFinance.Infrastructure.Data;

namespace StudentFinance.Infrastructure.Repositories
{
    public class FamilyRepository : GenericRepository<Family>, IFamilyRepository
    {
        public FamilyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Family?> GetFamilyWithMembersAsync(Guid familyId, CancellationToken cancellationToken)
        {
            // Include performs SQL JOIN to retrieve a list of members associated with this family
            return await _dbSet.AsNoTracking().Include(f => f.Members)
                                      .FirstOrDefaultAsync(f => f.Id == familyId, cancellationToken);
        }
    }
}
