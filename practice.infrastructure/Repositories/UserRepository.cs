using Microsoft.EntityFrameworkCore;
using practice.domain.Entities;
using practice.domain.Repositories;

namespace practice.infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }
    public User Add(User entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public bool Delete(string email)
    {
        var user = Get(email);
        if(string.IsNullOrEmpty(user.Email)==false)
        {
            _context.Remove(user);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public User Get(string email)
    {
        var user = _context.Users.Where(x => x.Email == email).FirstOrDefault();
        if(user ==null)
            return User.NoUser();
        return user;
    }

    public IEnumerable<User> List()
        => _context.Users.Where(x => x.Status == 1).ToList();

    public User Update(User entity)
    {
        var user = Get(entity.Email);
        if(string.IsNullOrEmpty(user.Email)==false)
        {
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }
        return User.NoUser();
    }
}