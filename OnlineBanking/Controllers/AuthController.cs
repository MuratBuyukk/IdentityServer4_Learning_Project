using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OnlineBanking.Controllers
{
    public class AuthController : Controller
    {
        public async Task<IActionResult> NewAccessToken()
        {
            string refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            HttpClient httpClient = new HttpClient();
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId = "OnlineBanking",
                ClientSecret = "OnlineBanking",
                RefreshToken = refreshToken,
                Address = (await httpClient.GetDiscoveryDocumentAsync("https://localhost:1000")).TokenEndpoint
            };
            TokenResponse tokenResponse = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
            AuthenticationProperties properties = (await HttpContext.AuthenticateAsync()).Properties;

            properties.StoreTokens(
                new List<AuthenticationToken> {
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.IdToken,
                                         Value = tokenResponse.IdentityToken
                                     },
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.AccessToken,
                                         Value = tokenResponse.AccessToken
                                     },
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.RefreshToken,
                                         Value = tokenResponse.RefreshToken
                                     },
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.ExpiresIn,
                                         Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("O")
                                     },
                                       });
            await HttpContext.SignInAsync("OnlineBankingCookie", (await HttpContext.AuthenticateAsync()).Principal, properties);
            return RedirectToAction(nameof(BankingController.Index));
        }
        public async Task<IActionResult> Index()
        {
            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync();
            IOrderedEnumerable<KeyValuePair<string, string>> properties = authenticateResult.Properties.Items.OrderBy(p => p.Key);
            return View(properties);
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("OnlineBankingCookie");
            await HttpContext.SignOutAsync("oidc");       
        }
    }
}
