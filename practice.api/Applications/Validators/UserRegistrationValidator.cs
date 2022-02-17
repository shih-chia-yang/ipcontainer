using practice.api.Applications.Contract;

namespace practice.api.Applications.Validators;

public interface IValidator<TEntity>
where TEntity:class
{
    bool IsValid{ get; }
}

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

public class UserRegistrationValidator : AbstractValidator<UserRegistration>
{
    public UserRegistrationValidator(UserRegistration entity) : base(entity)
    {
    }

    protected override bool Valid()
    {
        if(_entity is null)
            return false;
        return string.IsNullOrEmpty(_entity.FirstName) != true &&
            string.IsNullOrEmpty(_entity.LastName) != true &&
            string.IsNullOrEmpty(_entity.Email) != true &&
            string.IsNullOrEmpty(_entity.Password) != true;
    }
}