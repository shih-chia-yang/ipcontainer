using practice.domain.Entities;
using practice.domain.Repositories;

namespace practice.infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public IEnumerable<User> List()
        => new List<User>()
        {
            User.CreateNew("andy","wang","andy@test.com"),
            User.CreateNew("bod","chen","bob@test.com"),
            User.CreateNew("carol","chang","carol@test.com")
        };
}