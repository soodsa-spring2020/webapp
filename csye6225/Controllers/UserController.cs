using System.Security.Claims;
using System.Threading.Tasks;
using csye6225.Models;
using csye6225.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csye6225.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]     
    public class UserController : ControllerBase
    {
        private IUserService _userService;
      
        public UserController(IUserService userService) 
        {
            _userService = userService;
        } 

        [HttpGet] 
        public async Task<IActionResult> GetAll() 
        {     
            var users = await _userService.GetAll();
            return Ok(users);
        } 
 
        [Authorize]
        [HttpGet("self")] 
        public async Task<IActionResult> Get() 
        {    
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.Self(id);

            if (user == null)
                return BadRequest(new { message = "Network error. User could not be found." });

            return Ok(user);
        }

        [HttpPost] 
        public async Task<IActionResult> Create([FromBody]AccountCreateRequest req) 
        {    
            //User with same email already exists
            if(await _userService.CheckIfUserExists(req.email_address.Trim())) {
                return BadRequest(new { message = "User with same email already exists." });
            }

            var user = await _userService.Create(req);

            if (user == null)
                return BadRequest(new { message = "Network error. User could not be created.." });

            return Created(string.Empty, user);
        }

        [Authorize]
        [HttpPut("self")] 
        public async Task<IActionResult> Update([FromBody]AccountUpdateRequest req) 
        {    
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.Update(id, req);

            if (user == null)
                return BadRequest(new { message = "Network error. User could not be updated." });

            return NoContent();
        }
    }
}