using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using practice.domain.AggregateModel.Entities;
using practice.domain.Kernel.Repository;
using practice.domain.Repositories;

namespace practice.infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly UserContext _context;
    private readonly ILogger<RefreshTokenRepository> _logger;
    public IUnitOfWork UnitOfWork => _context;

    public RefreshTokenRepository(UserContext context,
    ILogger<RefreshTokenRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<RefreshToken> Get(string refreshToken)
    {
        try
        {
            return await _context.RefreshTokens
                .Where(x=>x.Token.ToLower()==refreshToken.ToLower())
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,$"{typeof(RefreshTokenRepository)} get method error ");
            return null;
        }
            
    }

    public async Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken)
    {
        try
        {
            var token= await _context.RefreshTokens
                .Where(x=>x.Token.ToLower()==refreshToken.Token.ToLower())
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if(token==null)return false;
            token.IsActive = refreshToken.IsActive;
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,$"{typeof(RefreshTokenRepository)} MarkRefreshTokenAsUsed method error ");
            return false;
        }
    }

    public Task<IEnumerable<RefreshToken>> ListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<RefreshToken> AddAsync(RefreshToken entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public Task<bool> RemoveAsync(Guid id, string approved)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken> UpdateAsync(RefreshToken entity)
    {
        throw new NotImplementedException();
    }
}
