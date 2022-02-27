using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practice.api.Applications.Contract;
using practice.api.Applications.Services;
using practice.api.Applications.Validators;

namespace practice.api.v2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly IAuthenticateManager _authenticateManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repo"></param>
        /// <param name="authenticateManager"></param>
        /// <param name="eventBus"></param>
        /// <param name="mapper"></param>
        public AccountController(
            ILogger<AccountController> logger,
            IUserRepository repo,
            IAuthenticateManager authenticateManager,
            IEventBus eventBus,
            IMapper mapper
            )
        {
            _logger = logger;
            _repo = repo;
            _eventBus = eventBus;
            _authenticateManager = authenticateManager;
            _mapper = mapper;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
            var result = await _authenticateManager.ValidateCredentials(request.Email, request.Password);
            if (result.Success is false)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistration request)
        {
            //check the model filed we are recicing is valid
            var validator = new UserRegistrationValidator(request);
            if(validator.IsValid)
            {
                //check if the user id or email exist in db
                var validEmail =_authenticateManager.Exist(request.Email);
                if(validEmail.Success is false)
                    return BadRequest(validEmail);
                // add user
                var result =await _eventBus.Send(new AddUserCommand
                {
                    FirstName=request.FirstName,
                    LastName=request.LastName,
                    Email=request.Email,
                    Password=request.Password
                });
                var newUser = result.Value as UserProfileViewModel;
                //create a jwt token
                var token =await _authenticateManager.ValidateCredentials(request.Email,request.Password);
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

        /// <summary>
        /// refresh token
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <returns></returns>
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
            var result =await _authenticateManager.RefreshToken(tokenRequest);
            if(result.Success is false)
                return BadRequest(result);
            return Ok(result);
        }
    }
}