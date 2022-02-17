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

namespace practice.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IOptions<JwtConfig> _config;
        private readonly IEventHandler<AddUserCommand, User> _addUser;

        public AccountController(
            IUserRepository repo,
            IOptions<JwtConfig> config,
            IEventHandler<AddUserCommand,User> addUser)
        {
            _repo = repo;
            _config = config;
            _addUser = addUser;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistration request)
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
                        new {Success=false,
                        Error=new List<string>(){
                        "Email already in use"
                        },
                        Token=string.Empty});
                }
                // add user
                var addUserCommand = new AddUserCommand
                {
                    FirstName=request.FirstName,
                    LastName=request.LastName,
                    Email=request.Email
                };
                var newUser=await _addUser.Handle(addUserCommand);
                //create a jwt token
                var token = GererateJwtToken(newUser);
                //return back to the user
                return Ok(new{
                    Success=true,
                    Token=token
                });
            }
            else
            {
                return BadRequest(new {
                    Success=false,
                    Error=new List<string>(){
                        "Invalid payload"
                    },
                    Token=string.Empty
                });
            }
            
        }

        private string GererateJwtToken(User user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            // get the security key
            var key = Encoding.UTF8.GetBytes(_config.Value.Secret);

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
                Expires=DateTime.UtcNow.AddMinutes(15), // todo update the expiration
                SigningCredentials=new SigningCredentials(
                    new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature) // todo review the algorithm
            };
            // generate the security token
            var token = jwtHandler.CreateToken(tokenDescripter);
            // convert the security obj token into a string
            var jwtToken = jwtHandler.WriteToken(token);

            return jwtToken;
        }
    }
}