using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GarantiAPI.Controllers
{
    [Route("api/garanti")]
    [ApiController]
    [Authorize]
    public class GarantiController : ControllerBase
    {
        [HttpGet("balance/{customerId}")]
        [Authorize(Policy = "ReadGaranti")]
        public ActionResult<double> Balance(int customerId)
        {
            // Retrieve balance logic
            double balance = 1000; // Replace with actual logic
            return Ok(balance);
        }

        [HttpGet("allaccounts/{customerId}")]
        [Authorize(Policy = "ReadGaranti")]
        public ActionResult<List<string>> AllAccounts(int customerId)
        {
            // Retrieve accounts logic
            var accounts = new List<string>
            {
                "123456789",
                "987654321",
                "564738291"
            };
            return Ok(accounts);
        }

        [HttpGet("deposite/{customerId}/{value}")]
        [Authorize(Policy = "AllGaranti")]
        public ActionResult<List<double>> Deposite(int customerId, double value)
        {
            value *= 0.5;
            return Ok(value);
        }
    }
}
