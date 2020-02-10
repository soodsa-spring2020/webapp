using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using csye6225.Helpers;
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
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bill = await _billService.Create(ownerId, req);

            if (bill == null)
                return BadRequest(new { message = "Network error. Bill could not be created." });

            return Created(string.Empty, bill);
        }

        [Authorize]
        [Route("v1/bills")] 
        [HttpGet] 
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
        [HttpDelete] 
        public async Task<IActionResult> Delete(string id) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isDeleted = await _billService.DeleteUserBill(ownerId, id);

            if (!isDeleted)
                return NotFound(new { message = "Bill not found." });

            return NoContent();
        }

        [Authorize]
        [Route("v1/bill")] 
        [HttpGet] 
        public async Task<IActionResult> Get(string id) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bill = await _billService.GetBill(ownerId, id);

            if (bill == null)
                return NotFound(new { message = "Bill not found." });

            return Ok(bill);
        }


        [Authorize]
        [Route("v1/bill")] 
        [HttpPut] 
        public async Task<IActionResult> Update(string id, [FromBody]BillUpdateRequest req) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _billService.Update(ownerId, id, req);

            if (user == null)
                return BadRequest(new { message = "Bill not found." });

            return NoContent();
        }

        [Authorize]
        [Route("v1/bill/{id}/file")] 
        [HttpPost] 
        public async Task<IActionResult> Attachment(string id, [FromForm]FileCreateRequest req) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Check if bill exists for the owner
            var bill = await _billService.GetBill(ownerId, id);
            if (bill == null)
                return NotFound(new { message = "Bill not found." });
            
            //Upload the file 
            var filePath = await FileHelper.UploadBillAttachment(id, req.file);
            if(string.IsNullOrEmpty(filePath)) {
                return BadRequest(new { message = "Bill attachment could not be uploaded." });
            }

            //File Uploaded Succesfully, Update the database
            FileInfo fileInfo = new FileInfo(filePath);
            var attachment = await _billService.StoreAttachment(id, fileInfo);

            if (attachment == null)
                return BadRequest(new { message = "Bill not found." });

             return Created(string.Empty, attachment);
        }
    }
}