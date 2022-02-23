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
using practice.api.Models.Dto;

namespace practice.api.v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly JwtConfig _config;
        private readonly IEventBus _eventBus;
        // private readonly IEventHandler<AddUserCommand, User> _addUser;

        public AccountController(
            IUserRepository repo,
            IOptions<JwtConfig> config,
            IEventBus eventBus
            )
        {
            _repo = repo;
            _config = config.Value;
            _eventBus = eventBus;
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
                        new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                        new Claim(JwtRegisteredClaimNames.Email,user.Email),
                        // used by the refresh token
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    }
                ),
                NotBefore=DateTime.UtcNow,
                Expires=DateTime.UtcNow.Add(TimeSpan.FromTicks(_config.ExpiryTimeFrame)), // todo update the expiration
                SigningCredentials=new SigningCredentials(
                    new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature) // todo review the algorithm
            };
            // generate the security token
            var token = jwtHandler.CreateToken(tokenDescripter);
            // convert the security obj token into a string
            var jwtToken = jwtHandler.WriteToken(token);
            // Generate a refresh token
            var refreshToken = new RefreshToken
                {
                    Created = DateTime.UtcNow,
                    Token = GenerateRandomString(25), //create a method to generate a random string and attach a certain guid
                    UserId = user.Id.ToString(),
                    IsRevoked = false,
                    IsActive = false,
                    JwtId=token.Id,
                    Expires=DateTime.UtcNow.AddMonths(6),
                };
            user.AddRefreshToken(refreshToken);
            _repo.Update(user);
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