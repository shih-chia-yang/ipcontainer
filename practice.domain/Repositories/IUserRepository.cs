using practice.domain.Entities;

namespace practice.domain.Repositories;

public interface IUserRepository
{
    IEnumerable<User> List();

    User Get(string email);
    User Add(User entity);

    User Update(User entity);

    bool Delete(string email);
}

public interface IAsyncUserRepository
{
    Task<IEnumerable<User>> ListAsync();

    Task<User> GetAsync(string email);
    Task<User> AddAsync(User entity);

    Task<User> UpdateAsync(User entity);

    Task<bool> DeleteAsync(string email);
}