using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HalkBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HalkbankController : ControllerBase
    {
        [HttpGet("balance/{customerId}")]
        public ActionResult<double> Balance(int customerId)
        {
            // Retrieve balance logic
            double balance = 500.15; // Replace with actual logic
            return Ok(balance);
        }

        [HttpGet("allaccounts/{customerId}")]
        public ActionResult<List<string>> AllAccounts(int customerId)
        {
            // Retrieve accounts logic
            var accounts = new List<string>
            {
                "135792468",
                "019283745",
                "085261060"
            };
            return Ok(accounts);
        }
    }
}
