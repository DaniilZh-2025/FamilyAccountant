using System.Data;
using FamilyAccountant.Application.DbConnection;
using FamilyAccountant.Domain.Entities;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Infrastructure.Extensions;

namespace FamilyAccountant.Infrastructure.Repositories;

public class UserRepository(IDbConnectionFactory dbFactory) : IUserRepository
{
    public async Task<bool> UserExist(string userLogin, IDbTransaction? transaction)
    {
        const string sql = """SELECT EXISTS(SELECT 1 FROM Usr WHERE Login = @UserLogin)""";
        
        return await sql.ExecuteScalarAsync<bool>(dbFactory, new { UserLogin = userLogin }, transaction);
    }

    public async Task<int> Create(User user, IDbTransaction? transaction)
    {
        const string sql = """
                            INSERT INTO Usr (Login, PasswordHash, HashSalt) VALUES (@Login, @PasswordHash, @HashSalt)
                            RETURNING Id
                            """;

        return await sql.ExecuteScalarAsync<int>(dbFactory, user, transaction);
    }

    public async Task<User?> FindBy(string userLogin, IDbTransaction? transaction)
    {
        const string sql = """
                            SELECT Id, Login, PasswordHash, HashSalt, RefreshTokenHash, RefreshTokenUtcExpiresAt, FamilyId, IsAdmin
                            FROM Usr
                            WHERE login = @UserLogin
                            """;
        
        return await sql.QuerySingleOrDefaultAsync<User>(dbFactory, new { UserLogin = userLogin }, transaction);
    }

    public async Task UpdateRefreshToken(User user, IDbTransaction? transaction)
    {
        using var connection = transaction?.Connection ?? dbFactory.CreateConnection();

        const string sql = """
                           UPDATE Usr
                           SET RefreshTokenHash = @RefreshTokenHash, RefreshTokenUtcExpiresAt = @RefreshTokenUtcExpiresAt
                           WHERE id = @Id
                           """;
        
        await sql.ExecuteAsync(dbFactory, user, transaction);
    }

    public async Task<int?> GetId(string userLogin, IDbTransaction? transaction)
    {
        const string sql = """
                           SELECT Id
                           FROM Usr
                           WHERE login = @UserLogin
                           """;
        
        return await sql.ExecuteScalarAsync<int>(dbFactory, new { UserLogin = userLogin }, transaction);
    }
    
    public async Task UpdateFamily(int userId, int familyId, bool isAdmin, IDbTransaction? transaction)
    {
        const string sql = """
                           UPDATE Usr
                           SET FamilyId = @FamilyId, IsAdmin = @IsAdmin
                           WHERE id = @Id
                           """;
        
        await sql.ExecuteAsync(dbFactory, new { Id = userId, FamilyId = familyId, IsAdmin = isAdmin }, transaction);
    }

    public async Task RemoveFamily(int userId, IDbTransaction? transaction)
    {
        const string sql = """
                           UPDATE Usr
                           SET FamilyId = NULL, IsAdmin = NULL
                           WHERE id = @Id
                           """;
        
        await sql.ExecuteAsync(dbFactory, new { Id = userId }, transaction);
    }
}
