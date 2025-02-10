using System.Data;

namespace FamilyAccountant.Application.DbConnection;

public interface IUnitOfWork : IDisposable
{
    IDbTransaction Transaction { get; }
    void BeginTransaction();
    void Commit();
    void Rollback();
}