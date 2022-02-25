using practice.api.Applications.Contract;

namespace practice.api.Applications.Services;

/// <summary>
/// 
/// </summary>
public interface IAuthenticateManager
{
    /// <summary>
    /// 驗證帳號密碼
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<AuthResult> ValidateCredentials(string email, string password);

    Task<AuthResult> RefreshToken(TokenRequest tokenRequest);

    AuthResult Exist(string email);
}