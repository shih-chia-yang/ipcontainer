using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice.domain.Entities;
using practice.domain.Repositories;

namespace practice.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        [Route("testrun")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>),StatusCodes.Status200OK)]
        public IActionResult TestRun()
        {
            var users = _repo.List();
            return Ok(users);
        }
    }
}