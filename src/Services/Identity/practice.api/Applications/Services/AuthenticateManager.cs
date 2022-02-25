using practice.api.Applications.Contract;

namespace practice.api.Applications.Services;

/// <summary>
/// 
/// </summary>
public class AuthenticateManager : IAuthenticateManager
{
    private readonly IUserRepository _repo;
    private readonly ITokenService _token;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repo"></param>
    public AuthenticateManager(
        IUserRepository repo,
        ITokenService token)
    {
        _repo = repo;
        _token = token;
    }

    public AuthResult Exist(string email)
    {
        var userExist = _repo.Get(email);
        if (userExist.IsEmpty is false)
            return new AuthResult
            {
                Success = false,
                Errors = new List<string>(){
                        "Email already in use"
                        },
                Token = string.Empty
            };
        return new AuthResult
        {
            Success = true,
        };
    }

    public Task<AuthResult> RefreshToken(TokenRequest tokenRequest)
        => _token.Verify(tokenRequest);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<AuthResult> ValidateCredentials(string email, string password)
    {
        var user = _repo.Get(email);
        if(user.IsEmpty is true || user.PasswordHash != password)
            return await Task.FromResult(new AuthResult
                {   
                    Token=string.Empty,
                    Success=false,
                    Errors=new List<string>()
                    {
                        "invalid authentication request"
                    }
                });
        
        return await _token.Generate(user);
    }
}