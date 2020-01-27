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
    public class BillController : ControllerBase
    {
        
        private IBillService _billService;
      
        public BillController(IBillService billService) 
        {
            _billService = billService;
        } 

        [Authorize]
        [HttpPost] 
        public async Task<IActionResult> Create([FromBody]BillCreateRequest req) 
        {    
            req.owner_id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bill = await _billService.Create(req);

            if (bill == null)
                return BadRequest(new { message = "Network error. Bill could not be created." });

            return Created(string.Empty, bill);
        }


    }
}