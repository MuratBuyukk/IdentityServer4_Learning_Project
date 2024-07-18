using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBanking.Controllers
{
    public class BankingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MakePayment()
        {

            var authenticationProperties = (await HttpContext.AuthenticateAsync()).Properties.Items;
            string accessToken = authenticationProperties.FirstOrDefault(x => x.Key == ".Token.access_token").Value;
            string refreshToken = authenticationProperties.FirstOrDefault(x => x.Key == ".Token.refresh_token").Value;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            HttpResponseMessage responseMessage = await httpClient.GetAsync("https://localhost:2000/api/garanti/balance/3");
            if (responseMessage.IsSuccessStatusCode)
            {
                string balance = await responseMessage.Content.ReadAsStringAsync();
                ViewBag.Balance = balance;
            }
            else
            {
                ViewBag.Balance = "API request failed..!";
            }

            ViewBag.AccesToken = accessToken;
            ViewBag.RefreshToken = refreshToken;
            return View();
        }
    }
}
