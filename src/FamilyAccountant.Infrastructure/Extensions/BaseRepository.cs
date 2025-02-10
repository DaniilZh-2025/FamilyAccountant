using System.Data;
using Dapper;
using FamilyAccountant.Application.DbConnection;

namespace FamilyAccountant.Infrastructure.Extensions;

public static class DapperExtensions
{
    public static async Task<int> ExecuteAsync(this string sql, IDbConnectionFactory dbFactory, object? param = null, IDbTransaction? transaction = null)
    {
        if (transaction?.Connection != null)
            return await transaction.Connection.ExecuteAsync(sql, param, transaction);
        
        using var connection = dbFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, param, transaction);
    }
    
    public static async Task<T?> ExecuteScalarAsync<T>(this string sql, IDbConnectionFactory dbFactory, object? param = null, IDbTransaction? transaction = null)
    {
        if (transaction?.Connection != null)
            return await transaction.Connection.ExecuteScalarAsync<T>(sql, param, transaction);
        
        using var connection = dbFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<T>(sql, param, transaction);
    }
    
    public static async Task<T?> QuerySingleOrDefaultAsync<T>(this string sql, IDbConnectionFactory dbFactory, object? param = null, IDbTransaction? transaction = null)
    {
        if (transaction?.Connection != null)
            return await transaction.Connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction);
        
        using var connection = dbFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction);
    }
    
    public static async Task<IEnumerable<T>> QueryAsync<T>(this string sql, IDbConnectionFactory dbFactory, object? param = null, IDbTransaction? transaction = null)
    {
        if (transaction?.Connection != null)
            return await transaction.Connection.QueryAsync<T>(sql, param, transaction);
        
        using var connection = dbFactory.CreateConnection();
        return await connection.QueryAsync<T>(sql, param, transaction);
    }
}