using practice.api.Applications.Contract;
using practice.domain.Kernel.Validator;

namespace practice.api.Applications.Validators;

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