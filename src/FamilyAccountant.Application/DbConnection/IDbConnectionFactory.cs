using System.Data;

namespace FamilyAccountant.Application.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}