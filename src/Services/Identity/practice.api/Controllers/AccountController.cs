using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using practice.api.Applications.Contract;
using practice.api.Applications.Validators;
using practice.api.Configuration.Models;

namespace practice.api.v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserRepository _repo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly JwtConfig _config;
        private readonly IEventBus _eventBus;
        private readonly TokenValidationParameters _tokenValidationParameters;

        // private readonly IEventHandler<AddUserCommand, User> _addUser;

        public AccountController(
            ILogger<AccountController> logger,
            IUserRepository repo,
            IRefreshTokenRepository refreshTokenRepo,
            IOptions<JwtConfig> config,
            TokenValidationParameters tokenValidationParameters,
            IEventBus eventBus
            )
        {
            _logger = logger;
            _repo = repo;
            _refreshTokenRepo = refreshTokenRepo;
            _config = config.Value;
            _eventBus = eventBus;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLogin request)
        {
            var validator = new UserLoginValidator(request);
            if(validator.IsValid is false)
            {
                return BadRequest(new AuthResult
                {   
                    Token=string.Empty,
                    Success=false,
                    Errors=new List<string>()
                    {
                        "invalid payload"
                    }
                });
            }
            var user = _repo.Get(request.Email);
            if(user.IsEmpty is true)
                    return BadRequest(new AuthResult
                {   
                    Token=string.Empty,
                    Success=false,
                    Errors=new List<string>()
                    {
                        "invalid authentication request"
                    }
                });

            if(user.PasswordHash != request.Password)
                return BadRequest(new AuthResult
                {   
                    Token=string.Empty,
                    Success=false,
                    Errors=new List<string>()
                    {
                        "invalid authentication request"
                    }
                });
            var jwtToken = await GererateJwtToken(user);
            return Ok(jwtToken);
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistration request)
        {
            //check the model filed we are recicing is valid
            var validator = new UserRegistrationValidator(request);
            if(validator.IsValid)
            {
                //check if the user id or email exist in db
                var userExist = _repo.Get(request.Email);
                if(string.IsNullOrEmpty(userExist.Email)==false)
                {
                    return BadRequest(
                        new AuthResult{Success=false,
                        Errors=new List<string>(){
                        "Email already in use"
                        },
                        Token=string.Empty});
                }
                // add user
                // var newUser=await _addUser.Handle(addUserCommand);
                var result =await _eventBus.Send(new AddUserCommand
                {
                    FirstName=request.FirstName,
                    LastName=request.LastName,
                    Email=request.Email,
                    Password=request.Password
                });
                var newUser = result.Value as User;
                //create a jwt token
                var token = await GererateJwtToken(newUser);
                
                
                //return back to the user
                return Ok(token);
            }
            else
            {
                return BadRequest(new AuthResult{
                    Success=false,
                    Errors=new List<string>(){
                        "Invalid payload"
                    },
                    Token=string.Empty
                });
            }
            
        }

        [Route("refreshtoken")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var validator = new TokenRequestValidator(tokenRequest);
            if(validator.IsValid is false)
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Token=string.Empty,
                    Errors=new List<string>()
                    {
                        "invalid payload"
                    }
                });

            // check token is valid
            var result =await VerifyTokenAsync(tokenRequest);
            if(result.Success is false)
                return BadRequest(new AuthResult()
                {
                    Success=false,
                    Errors=new List<string>()
                    {
                        "token validation failed"
                    }
                });
            return Ok(result);
        }

        private async Task<AuthResult> VerifyTokenAsync(TokenRequest tokenRequest)
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
                        var tokens = await GererateJwtToken(user);
                        return new AuthResult()
                        {
                            Success = true,
                            Token = tokens.Token,
                            RefreshToken = tokens.RefreshToken
                        };
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

        private DateTime UnixTimeStampToDateTime(long utcExpiryDate)
        {
            // set the time to original time format
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            // add the number of seconds from original time
            return dateTime.AddSeconds(utcExpiryDate).ToUniversalTime();
        }

        private async Task<AuthResult> GererateJwtToken(User user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            // get the security key
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
            // Generate a refresh token
            var refreshToken = new RefreshToken
                {
                    JwtId=token.Id,
                    IsActive = false,
                    IsRevoked = false,
                    UserId = user.Id.ToString(),
                    Created = DateTime.UtcNow,
                    Expires=DateTime.UtcNow.AddMonths(6),
                    Token = GenerateRandomString(25), //create a method to generate a random string and attach a certain guid
                };
            // user.AddRefreshToken(refreshToken);
            await _refreshTokenRepo.AddAsync(refreshToken);
            await _repo.UnitOfWork.SaveChangesAsync();
            return new AuthResult
            {
                Token = jwtToken,
                Success = true,
                RefreshToken=refreshToken.Token,
            };
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCEDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklknopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(x=>x[random.Next(x.Length)]).ToArray());
        }
    }
}