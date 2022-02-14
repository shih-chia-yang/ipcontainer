namespace practice.domain.Kernel.Repository;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}