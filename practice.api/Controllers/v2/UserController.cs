using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace practice.api.Controllers.v2
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("2.0",Deprecated =true)]
    public class UserController : ControllerBase
    {
        [Route("users")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok("this is users api version2.0");
        }
    }
}