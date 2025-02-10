using System.Data;
using FamilyAccountant.Application.DbConnection;
using Npgsql;

namespace FamilyAccountant.Infrastructure.DbConnection;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
}