using Microsoft.AspNetCore.Mvc;

namespace OnlineBanking.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
