using practice.api.Applications.Contract;
namespace practice.api.Applications.Validators;

public class UserLoginValidator : AbstractValidator<UserLogin>
{
    public UserLoginValidator(UserLogin entity):base(entity)
    {

    }
    protected override bool Valid()
        => string.IsNullOrEmpty(_entity.Email) is false &&
        string.IsNullOrEmpty(_entity.Password) is false;
}
