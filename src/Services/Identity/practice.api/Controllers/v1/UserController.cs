using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace practice.api.v1.Controllers
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
{
        private readonly IUserRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public UserController(
            IUserRepository repo,
            IEventBus eventBus,
            IMapper mapper)
        {
            _repo = repo;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("users")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserProfileViewModel>),StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            var users = _repo.List().Where(x=>x.Status==1).ToList();
            var viewModels=_mapper.Map<IEnumerable<UserProfileViewModel>>(users);
            return Ok(viewModels);
        }

        [Route("user")]
        [HttpPost]
        [ProducesResponseType(typeof(UserProfileViewModel),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody]AddUserCommand command)
        {
            var result =await _eventBus.Send(command);
            if(result.Success is true)
                return Ok(result.Value);
            else
            {
                var response = new Response<UserProfileViewModel>
                    (ResponseError.CreateNew(400,"",nameof(BadRequest)));

                return BadRequest(response);
            }
        }

        [Route("user")]
        [HttpPut]
        [ProducesResponseType(typeof(UserProfileViewModel),StatusCodes.Status202Accepted)]
        public async Task<IActionResult> UpdateAsync([FromBody]UpdateUserCommand command)
        {
            var result =await _eventBus.Send(command);
            if(result.Success is true)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Route("user")]
        [HttpDelete]
        [ProducesResponseType(typeof(bool),StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteAsync([FromQuery]DeleteUserCommand command)
        {
            var result =await _eventBus.Send(command);
            if(result.Success is true)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}