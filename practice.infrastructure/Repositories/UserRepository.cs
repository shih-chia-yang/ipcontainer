using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using practice.domain.Entities;
using practice.domain.Kernel.Repository;
using practice.domain.Repositories;

namespace practice.infrastructure.Repositories;

public class UserRepository : IUserRepository,IAsyncUserRepository
{
    private readonly UserContext _context;
    private readonly ILogger<UserRepository> _logger;

    public IUnitOfWork UnitOfWork =>_context;


    public UserRepository(UserContext context,
    ILogger<UserRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }
    public User Add(User entity)
    {
        _context.Add(entity);
        return entity;
    }

    public bool Delete(string email)
    {
        var user = Get(email);
        if(string.IsNullOrEmpty(user.Email)==false)
        {
            _context.Remove(user);
            return true;
        }
        return false;
    }

    public User Get(string email)
    {
        var user = _context.Users
            .Where(x => x.Email == email)
            .AsNoTracking()
            .FirstOrDefault();
        if(user ==null)
            return User.NoUser();
        return user;
    }

    public async Task<User> GetAsync(Guid id)
    {
        var user = await _context.Users
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if(user==null)
            return User.NoUser();
        return user;
    }

    public IEnumerable<User> List()
    {
        try
        {
            return _context.Users.Where(x => x.Status == 1).ToList();
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex,$"{typeof(UserRepository)} List method has generated error");
            return Enumerable.Empty<User>();
        }
    }

    public async Task<bool> RemoveAsync(Guid id, string approved)
    {
        var user =await GetAsync(id);
        if(string.IsNullOrEmpty(user.Email)==false)
        {
            _context.Remove(user);
            return true;
        }
        return false;
    }

    public User Update(User entity)
    {
        var user = Get(entity.Email);
        if(string.IsNullOrEmpty(user.Email)==false)
        {
            _context.Update(user);
            return user;
        }
        return User.NoUser();
    }

    public async Task<User>AddAsync(User entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<User>> ListAsync()
    {
        try
        {
            return await _context.Users
                .Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex,$"{typeof(UserRepository)} ListAsync method has generated error");
            return Enumerable.Empty<User>();
        }
    }
        

    public async Task<User> UpdateAsync(User entity)
    {
        var user = await GetAsync(entity.Id);
        if(string.IsNullOrEmpty(user.Email)==false)
        {
            _context.Update(user);
            return user;
        }
        return User.NoUser();
    }
}