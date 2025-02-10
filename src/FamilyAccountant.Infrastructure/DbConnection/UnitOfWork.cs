using System.Data;
using FamilyAccountant.Application.DbConnection;

namespace FamilyAccountant.Infrastructure.DbConnection;

public class UnitOfWork(IDbConnectionFactory dbConnectionFactory) : IUnitOfWork
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    public IDbTransaction Transaction => _transaction
                                         ?? throw new InvalidOperationException("Transaction not started");

    public void BeginTransaction()
    {
        _connection = dbConnectionFactory.CreateConnection();
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started");

        _transaction.Commit();
        Dispose();
    }

    public void Rollback()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started");

        _transaction.Rollback();
        Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
        _disposed = true;
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}