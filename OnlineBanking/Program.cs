using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(_ =>
{
    _.DefaultScheme = "OnlineBankingCookie";
    _.DefaultChallengeScheme = "oidc";
})
    .AddCookie("OnlineBankingCookie")
    .AddOpenIdConnect("oidc", _ =>
    {
        _.SignInScheme = "OnlineBankingCookie"; // iþleminin yapacaðý þemayý temsil eder. ‘AddAuthentication’ metodundaki ‘DefaultScheme’ ile ayný olmalýdýr.
        _.Authority = "https://localhost:1000"; // Yetkinin kimden alýndýðýný tutar. ‘AuthServer’ýn adresini belirtiyoruz.
        _.ClientId = "OnlineBanking"; // Bu client’ýn ‘AuthServer’da ki Client_Id karþýlýðý.
        _.ClientSecret = "OnlineBanking"; // Bu client’ýn ‘AuthServer’da ki Client_Secret karþýlýðý.
        _.ResponseType = "code id_token"; // Oluþturulacak Authorization Code içerisinde bulunmasýný istediðimiz datalarý belirtiyoruz. ‘code’, üretilecek olan authorize kodu ifade ederken,
                                          // ‘id_token’ ise access token’ýn bize ait olan Auth Server’dan gelip gelmediðini test etmek için üretilen bir deðerdir.
        _.GetClaimsFromUserInfoEndpoint = true; // Ek taleplerin (claims) UserInfo Endpoint'inden alýnacaðýný belirtir. Bu, kimlik belirtecinde (ID token) bulunmayan 
                                                // kullanýcý bilgilerini almak için kullanýlýr. (opsiyonel)
        _.SaveTokens = true; // Access Token ve Id Token'ýn Elde edilmesini saðlar (opsiyonel)
        _.Scope.Add("offline_access");
        _.Scope.Add("Garanti.Write");
        _.Scope.Add("Garanti.Read");
        _.Scope.Add("HalkBank.Write");
        _.Scope.Add("HalkBank.Read");
        _.Scope.Add("PositionAndAuthority");
        _.ClaimActions.MapUniqueJsonKey("position", "position");
        _.ClaimActions.MapUniqueJsonKey("authority", "authority");
        _.Scope.Add("Roles");
        _.ClaimActions.MapUniqueJsonKey("role", "role");
        _.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = "role"
        };
        _.Scope.Add("Nickname");
        _.ClaimActions.MapUniqueJsonKey("nickname","nickname");
    });



builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Banking}/{action=Index}/{id?}");


app.Run();
