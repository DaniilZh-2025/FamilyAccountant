using System.Data;
using FamilyAccountant.Domain.Entities;

namespace FamilyAccountant.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> UserExist(string userLogin, IDbTransaction? transaction = null);
    Task<int> Create(User user, IDbTransaction? transaction = null);
    Task<User?> FindBy(string userLogin, IDbTransaction? transaction = null);
    Task UpdateRefreshToken(User user, IDbTransaction? transaction = null);
    Task<int?> GetId(string userLogin, IDbTransaction? transaction = null);
    Task UpdateFamily(int userId, int familyId, bool isAdmin, IDbTransaction? transaction = null);
    Task RemoveFamily(int userId, IDbTransaction? transaction = null);
}