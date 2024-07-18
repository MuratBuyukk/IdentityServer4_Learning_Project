using IdentityServer4.Models;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, "murat_buyuk_1999@hotmail.com"),
                new Claim(JwtRegisteredClaimNames.Website, "https://github.com/MuratBuyukk"),
                new Claim("nickname", "muratbuyukk")
            };

            context.AddRequestedClaims(claims);
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
