namespace practice.domain.Kernel.Validator;

public interface IValidator<TEntity>
where TEntity:class
{
    bool IsValid{ get; }
}