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
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
{
        private readonly IUserRepository _repo;
        private readonly IEventHandler<AddUserCommand,User> _addHandler;
        private readonly IEventHandler<UpdateUserCommand, User> _updateHandler;
        private readonly IEventHandler<DeleteUserCommand, bool> _deleteHandler;

        public UserController(IUserRepository repo,
        IEventHandler<AddUserCommand, User> addHandler,
        IEventHandler<UpdateUserCommand, User> updateHandler
        , IEventHandler<DeleteUserCommand, bool> deleteHandler)
        {
            _repo = repo;
            _addHandler = addHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
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
        public IActionResult Add([FromBody]AddUserCommand command)
        {
            var user = _addHandler.HandleAsync(command);
            return Ok(user);
        }

        [Route("user")]
        [HttpPut]
        [ProducesResponseType(typeof(User),StatusCodes.Status202Accepted)]
        public IActionResult Update([FromBody]UpdateUserCommand command)
        {
            var updateUser = _updateHandler.HandleAsync(command);
            return Ok(updateUser);
        }

        [Route("user")]
        [HttpDelete]
        [ProducesResponseType(typeof(bool),StatusCodes.Status202Accepted)]
        public IActionResult Delete([FromQuery]DeleteUserCommand command)
        {
            var succeeded = _deleteHandler.HandleAsync(command);
            return Ok(succeeded);
        }
    }
}