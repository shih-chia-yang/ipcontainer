using practice.api.Applications.Contract;
using practice.domain.Kernel.Validator;

namespace practice.api.Applications.Validators;

public class UserLoginValidator : AbstractValidator<UserLogin>
{
    public UserLoginValidator(UserLogin entity):base(entity)
    {
        if(entity is null)
            throw new ArgumentNullException(nameof(UserLogin));
    }
    protected override bool Valid()
        => string.IsNullOrEmpty(_entity.Email) is false &&
        string.IsNullOrEmpty(_entity.Password) is false;
}
