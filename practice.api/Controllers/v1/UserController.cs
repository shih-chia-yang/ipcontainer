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

        public UserController(
            IUserRepository repo,
            IEventBus eventBus)
        {
            _repo = repo;
            _eventBus = eventBus;
        }

        [AllowAnonymous]
        [Route("users")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>),StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            var users = _repo.List().Where(x=>x.Status==1).ToList();
            return Ok(users);
        }

        [Route("user")]
        [HttpPost]
        [ProducesResponseType(typeof(User),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody]AddUserCommand command)
        {
            var result =await _eventBus.Send(command);
            if(result.Success is true)
                return Ok(result.Value);
            else
                return BadRequest(result.Errors);
        }

        [Route("user")]
        [HttpPut]
        [ProducesResponseType(typeof(User),StatusCodes.Status202Accepted)]
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