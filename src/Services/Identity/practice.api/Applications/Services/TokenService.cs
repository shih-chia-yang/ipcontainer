using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using practice.api.Applications.Contract;
using practice.api.Configuration.Models;

namespace practice.api.Applications.Services;

/// <summary>
/// 
/// </summary>
public class TokenService :ITokenService
{
    private readonly IUserRepository _repo;
    private readonly JwtConfig _config;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IRefreshTokenRepository _refreshTokenRepo;

    public TokenService(
        IUserRepository repo,
        IOptions<JwtConfig> config,
        TokenValidationParameters tokenValidationParameters,
        IRefreshTokenRepository refreshTokenRepo)
    {
        _repo = repo;
        _config = config.Value;
        _tokenValidationParameters = tokenValidationParameters;
        _refreshTokenRepo = refreshTokenRepo;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<AuthResult> Generate(User user)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config.Secret);
        var tokenDescripter = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id",user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    // used by the refresh token
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                }
            ),
            Expires=DateTime.UtcNow.AddSeconds(_config.ExpiryTimeFrame), // todo update the expiration
            SigningCredentials=new SigningCredentials(
                new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256) // todo review the algorithm
        };
        // generate the security token
        var token = jwtHandler.CreateToken(tokenDescripter);
        // convert the security obj token into a string
        var jwtToken = jwtHandler.WriteToken(token);
        var refreshToken=await GetRefreshToken(token.Id,user);
        return new AuthResult
            {
                Token = jwtToken,
                Success = true,
                RefreshToken=refreshToken.Token,
            };
    }

    private DateTime UnixTimeStampToDateTime(long utcExpiryDate)
    {
        // set the time to original time format
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        // add the number of seconds from original time
        return dateTime.AddSeconds(utcExpiryDate).ToUniversalTime();
    }
    private string GenerateRandomString(int length)
    {
        var random = new Random();
        var chars = "ABCEDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklknopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(x=>x[random.Next(x.Length)]).ToArray());
    }

    public async Task<RefreshToken> GetRefreshToken(string tokenId,User user)
    {
        var refreshToken = new RefreshToken
            {
                JwtId=tokenId,
                IsActive = false,
                IsRevoked = false,
                UserId = user.Id.ToString(),
                Created = DateTime.UtcNow,
                Expires=DateTime.UtcNow.AddMonths(6),
                Token = GenerateRandomString(25), //create a method to generate a random string and attach a certain guid
            };
            // user.AddRefreshToken(refreshToken);
        await _refreshTokenRepo.AddAsync(refreshToken);
        await _refreshTokenRepo.UnitOfWork.SaveChangesAsync();
        return await Task.FromResult(refreshToken);
    }

    public async Task<AuthResult> Verify(TokenRequest tokenRequest)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            // check the validity of the token
            var principal = tokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters,out var validatedToken);
            // validate the results that has been generated for us
            // validate if the string is an actual jwt token not a random string
            if(validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                // check if the jwt token is created with the same algorithm as generated token
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if(result is false)
                    return new AuthResult() { Success = false,Token=string.Empty };
            }
            // check expiry date of the token
            var utcExpiryDate = long.Parse(
                    principal.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Exp)
                    .FirstOrDefault()
                    .Value);
            // convert to date to check
            var expDate = UnixTimeStampToDateTime(utcExpiryDate);
            if(expDate>DateTime.UtcNow)
                return new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "token has not expired"
                    }
                };
            var refreshTokenExist = await _refreshTokenRepo.Get(tokenRequest.RefreshToken);
            if(refreshTokenExist ==null)
                return new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "Invalid refresh token"
                    }
                };
            // check the expiry date of a refresh token
            if(refreshTokenExist.Expires < DateTime.UtcNow)
                return new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "Refresh token has expired,please login again"
                    }
                };
            // check if refresh token has been used or not
            if(refreshTokenExist.IsActive)
                return new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "Refresh token has been used, it cannot be used"
                    }
                };
            
            // check refresh token if it has been revoked
            if(refreshTokenExist.IsRevoked)
                return new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "Refresh token has been revoked, it cannot be used"
                    }
                };

            var jti = principal.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Jti).SingleOrDefault().Value;
            if(refreshTokenExist.JwtId != jti)
                return new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "Refresh token refresh does not match the jwt token"
                    }
                };
                refreshTokenExist.IsActive = true;
                var updateResult =await _refreshTokenRepo.MarkRefreshTokenAsUsed(refreshTokenExist);
                if(updateResult is true)
                {
                    await _refreshTokenRepo.UnitOfWork.SaveChangesAsync();

                    // get the user to generatyet a new jwt token
                    var user = await _repo.GetAsync(Guid.Parse(refreshTokenExist.UserId));
                    if(user is null)
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Error processing request"
                            }
                        };
                    return await Generate(user);
                }
                else
                    return new AuthResult()
                    {
                        Success=false,
                        Errors=new List<string>()
                        {
                            "Error processing request"
                        }
                    };
        }
        catch(Exception ex)
        {
            // _logger.LogError(ex, $"{typeof()}");
            // todo: add better error handling, and add log
            Console.WriteLine(ex.Message);
            return new AuthResult()
            {
                Success=false,
                Errors=new List<string>()
                {
                    ex.Message
                }
            };
        }    
    }
}