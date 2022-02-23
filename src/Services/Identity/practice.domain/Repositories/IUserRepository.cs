using practice.domain.AggregateModel.Entities;
using practice.domain.Kernel.Repository;

namespace practice.domain.Repositories;

public interface IUserRepository:IRepository,IAsyncUserRepository
{
    IEnumerable<User> List();

    User Get(string email);

    User Add(User entity);

    User Update(User entity);

    bool Delete(string email);
}

public interface IAsyncUserRepository:IGenericRepository<User>
{
}