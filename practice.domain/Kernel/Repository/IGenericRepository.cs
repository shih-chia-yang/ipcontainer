namespace practice.domain.Kernel.Repository;

public interface IGenericRepository<TEntity>
where TEntity :class
{
    Task<IEnumerable<TEntity>> ListAsync();

    Task<TEntity> GetAsync(Guid id);

    Task<TEntity> AddAsync(TEntity entity);

    Task<bool> RemoveAsync(Guid id,string approved);

    // update entity of add if it doesn't exist
    Task<TEntity> UpdateAsync(TEntity entity);

}