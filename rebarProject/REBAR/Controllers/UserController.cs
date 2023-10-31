using Microsoft.AspNetCore.Mvc;
using REBAR.Models;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetUser(UserInput input)
        {
            if (string.IsNullOrEmpty(input.CustomerName))
            {
                return BadRequest("User name cannot be empty.");
            }

            // Continue processing the data here if the name is valid.
            return Ok("Name is valid.");
        }
    }

}
