using System.Security.Claims;
using System.Threading.Tasks;
using csye6225.Models;
using csye6225.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csye6225.Controllers
{
    [ApiController]
    public class BillController : ControllerBase
    {
        
        private IBillService _billService;
      
        public BillController(IBillService billService) 
        {
            _billService = billService;
        } 

        [Authorize]
        [HttpPost] 
        [Route("v1/bill")]   
        public async Task<IActionResult> Create([FromBody]BillCreateRequest req) 
        {    
            req.owner_id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bill = await _billService.Create(req);

            if (bill == null)
                return BadRequest(new { message = "Network error. Bill could not be created." });

            return Created(string.Empty, bill);
        }

        [Authorize]
        [Route("v1/bills")]  
        public async Task<IActionResult> Bills() 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bills = await _billService.GetUserBills(ownerId);

            if (bills == null)
                return BadRequest(new { message = "Network error. Bills could not be found." });

            return Ok(bills);
        }

        [Authorize]
        [Route("v1/bill")]  
        public async Task<IActionResult> Delete(string id) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isDeleted = await _billService.DeleteUserBill(ownerId, id);

            if (!isDeleted)
                return NotFound(new { message = "Bill not found." });

            return NoContent();
        }


    }
}