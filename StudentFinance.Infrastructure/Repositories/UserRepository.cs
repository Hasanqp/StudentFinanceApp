using Microsoft.EntityFrameworkCore;
using StudentFinance.Domain.Entities;
using StudentFinance.Domain.Interfaces.Repositories;
using StudentFinance.Infrastructure.Data;

namespace StudentFinance.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
