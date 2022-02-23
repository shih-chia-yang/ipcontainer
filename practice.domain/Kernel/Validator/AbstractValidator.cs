namespace practice.domain.Kernel.Validator;

public abstract class AbstractValidator<TEntity> : IValidator<TEntity>
where TEntity:class
{
    protected readonly TEntity? _entity;

    protected AbstractValidator(TEntity entity)
    {
        _entity = entity;
    }

    public bool IsValid => Valid();

    protected abstract bool Valid();
}