using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBanking.Controllers
{
    public class BalanceController : Controller
    {
        [Authorize(Roles = "admin")]
        public IActionResult CheckBalance()
        {
            return Ok(1000);
        }
        [Authorize(Roles = "moderator")]
        public IActionResult ShowTransactions()
        {
            return Ok("Transaction-1 \nTransaction-2\nTransaction-3");
        }
        [Authorize(Roles = "admin, moderator")]
        public IActionResult Print()
        {
            return Ok("Document Printed");
        }
    }
}
