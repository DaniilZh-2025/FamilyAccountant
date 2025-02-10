using System.Data;
using FamilyAccountant.Domain.Entities;

namespace FamilyAccountant.Domain.Repositories;

public interface IFamilyRepository
{
    Task<int> Create(IDbTransaction? transaction = null);
    Task<Family?> FindBy(int familyId, IDbTransaction? transaction = null);
    Task<IEnumerable<User>> GetMembers(int familyId, IDbTransaction? transaction  = null);
}