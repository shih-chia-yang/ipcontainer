using practice.domain.AggregateModel.Entities;
using practice.domain.Kernel.Repository;

namespace practice.domain.Repositories;

public interface IRefreshTokenRepository:IRepository,IGenericRepository<RefreshToken>
{
    Task<RefreshToken> Get(string refreshToken);
    Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken);
}