using System.Data;
using FamilyAccountant.Application.DbConnection;
using FamilyAccountant.Domain.Entities;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Infrastructure.Extensions;

namespace FamilyAccountant.Infrastructure.Repositories;

public class FamilyRepository(IDbConnectionFactory dbFactory) : IFamilyRepository
{
    public async Task<int> Create(IDbTransaction? transaction)
    {
        const string sql = """
                            INSERT INTO Family (FamilyLink) VALUES (@FamilyLink)
                            RETURNING Id
                            """;
        
        return await sql.ExecuteScalarAsync<int>(dbFactory, new { FamilyLink = Guid.NewGuid().ToString() }, transaction);
    }

    public async Task<Family?> FindBy(int familyId, IDbTransaction? transaction)
    {
        const string sql = "SELECT Id, FamilyLink FROM Family WHERE Id = @Id";
        
        return await sql.QuerySingleOrDefaultAsync<Family>(dbFactory, new { Id = familyId }, transaction);
    }

    public async Task<IEnumerable<User>> GetMembers(int familyId, IDbTransaction? transaction)
    {
        const string sql = """
                            SELECT u.Id, u.Login, u.FamilyId, u.IsAdmin
                            FROM Family f
                            JOIN Usr u ON f.Id = u.FamilyId
                            WHERE f.Id = @Id
                            """;
        
        return await sql.QueryAsync<User>(dbFactory, new { Id = familyId }, transaction);
    }
}