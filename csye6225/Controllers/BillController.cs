using System.IO;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.S3.Model;
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
        private IFileService _fileService;
        private INotificationService _notificationService;
      
        public BillController(IBillService billService, IFileService fileService,
        INotificationService notificationService) 
        {
            _billService = billService;
            _fileService = fileService;
            _notificationService = notificationService;
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
        [Route("v1/bills/due/{days}")] 
        [HttpGet] 
        public async Task<IActionResult> BillsDue(string days) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ownerEmail = this.User.FindFirstValue(ClaimTypes.Email);
            var bills = await _billService.GetUserDueBills(ownerId, days);

            if (bills == null)
                return BadRequest(new { message = "Network error. Bills could not be found." });
           
            _notificationService.AddToNotificationQueue(ownerEmail, bills);
            return Ok(bills);
        }

        [Authorize]
        [Route("v1/bill/{id}")] 
        [HttpDelete] 
        public async Task<IActionResult> Delete(string id) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Delete file information from the database
            var fileName = await _billService.DeleteAttachment(id);
            var isDeleted = await _billService.DeleteUserBill(ownerId, id);
            if (!isDeleted)
                return NotFound(new { message = "Bill not found." });

            //Remove file from the storage
            if(!string.IsNullOrEmpty(fileName))
                await _fileService.DeleteBillAttachment(id,fileName);
            return NoContent();
        }

        [Authorize]
        [Route("v1/bill/{id}")] 
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
        [Route("v1/bill/{id}")] 
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
        public async Task<IActionResult> UploadAttachment(string id, [FromForm]FileCreateRequest req) 
        {    
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Check if bill exists for the owner
            var bill = await _billService.GetBill(ownerId, id);
            if (bill == null)
                return NotFound(new { message = "Bill not found." });
            
            //Upload the file 
            GetObjectResponse fileInfo = await _fileService.UploadBillAttachment(id, req.file);
            if(fileInfo == null) {
                return BadRequest(new { message = "Bill attachment could not be uploaded." });
            }

            //File Uploaded Succesfully, Update the database
            var attachment = await _billService.StoreAttachment(id, fileInfo);
            if (attachment == null)
                return BadRequest(new { message = "Error saving attachment information." });

            return Created(string.Empty, attachment);
        }

        [Authorize]
        [Route("v1/bill/{billId}/file/{fileId}")] 
        [HttpGet] 
        public async Task<IActionResult> GetAttachment(string billId, string fileId)
        {
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bill = await _billService.GetBill(ownerId, billId);

            if (bill == null)
                return NotFound(new { message = "Bill not found." });

            if(bill.attachment == null || bill.attachment.id.ToString() != fileId)
                return NotFound(new { message = "Attachment not found." });

            return Ok(bill.attachment);
        }

        [Authorize]
        [Route("v1/bill/{billId}/file/{fileId}")] 
        [HttpDelete] 
        public async Task<IActionResult> DeleteAttachment(string billId, string fileId)
        {
            var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bill = await _billService.GetBill(ownerId, billId);

            if (bill == null)
                return NotFound(new { message = "Bill not found." });

            if(bill.attachment == null || bill.attachment.id.ToString() != fileId)
                return NotFound(new { message = "Attachment not found." });

            //Delete file information from the database
            var fileName = await _billService.DeleteAttachment(billId);
            if(string.IsNullOrEmpty(fileName))
                return BadRequest(new { message = "Error deleting attachment information." });
            
            //Remove file from the local storage
            await _fileService.DeleteBillAttachment(billId, fileName);

            return NoContent();
        }
    }
}