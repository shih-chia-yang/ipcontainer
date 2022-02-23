namespace practice.domain.Kernel.Validator;

public abstract class AbstractValidator<TEntity> : IValidator<TEntity>
where TEntity:class
{
    protected readonly TEntity? _entity;

    protected AbstractValidator(TEntity entity)
    {
        if(entity is null)
            throw new ArgumentNullException(nameof(entity));
        _entity = entity;
    }

    public bool IsValid => Valid();

    protected abstract bool Valid();
}