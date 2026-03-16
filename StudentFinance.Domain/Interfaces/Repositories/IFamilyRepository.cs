using StudentFinance.Domain.Entities;

namespace StudentFinance.Domain.Interfaces.Repositories
{
    public interface IFamilyRepository : IGenericRepository<Family>
    {
        Task<Family?> GetFamilyWithMembersAsync(Guid familyId);
    }
}
