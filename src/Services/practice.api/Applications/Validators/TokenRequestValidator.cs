using practice.api.Applications.Contract;


namespace practice.api.Applications.Validators;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator(TokenRequest entity) : base(entity)
    {

    }

    protected override bool Valid()
        =>string.IsNullOrEmpty(_entity.Token) is false &&
        string.IsNullOrEmpty(_entity.RefreshToken) is false;
}