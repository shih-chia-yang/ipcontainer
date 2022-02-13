using practice.domain.Entities;

namespace practice.domain.Repositories;

public interface IUserRepository
{
    IEnumerable<User> List();
}