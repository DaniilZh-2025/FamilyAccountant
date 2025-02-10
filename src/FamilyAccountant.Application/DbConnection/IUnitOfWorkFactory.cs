namespace FamilyAccountant.Application.DbConnection;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}