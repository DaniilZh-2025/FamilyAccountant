using FamilyAccountant.Application.DbConnection;

namespace FamilyAccountant.Infrastructure.DbConnection;

public class UnitOfWorkFactory(IDbConnectionFactory dbConnectionFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbConnectionFactory);
}