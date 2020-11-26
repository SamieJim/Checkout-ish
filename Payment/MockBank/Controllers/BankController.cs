using Microsoft.AspNetCore.Mvc;
using MockBank.Models;

namespace MockBank.Controllers
{
    public class BankController : ControllerBase
    {

        // We are very simply assuming that the bank pays the merchant
        public ObjectResult PaySuccess()
        {
            BankResponse success = new BankResponse
            {
                Id = System.Guid.NewGuid(),
                Status = 2
            };
            return Ok(success);      
        }

        // We are very simply assuming that the bank fails to pay the merchant
        [HttpGet("Fail")]
        public ActionResult PayFail()
        {
            BankResponse failure = new BankResponse
            {
                Id = System.Guid.NewGuid(),
                Status = 3
            };
            return BadRequest(failure);      
        }
        
    }
}