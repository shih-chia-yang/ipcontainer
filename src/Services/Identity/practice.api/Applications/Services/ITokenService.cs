using practice.api.Applications.Contract;

namespace practice.api.Applications.Services;

/// <summary>
/// 
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<AuthResult> Generate(User user);

    Task<RefreshToken> GetRefreshToken(string tokenId,User user);

    Task<AuthResult> Verify(TokenRequest request);
}