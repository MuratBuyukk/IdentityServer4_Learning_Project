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
        _.SignInScheme = "OnlineBankingCookie"; // i�leminin yapaca�� �emay� temsil eder. �AddAuthentication� metodundaki �DefaultScheme� ile ayn� olmal�d�r.
        _.Authority = "https://localhost:1000"; // Yetkinin kimden al�nd���n� tutar. �AuthServer��n adresini belirtiyoruz.
        _.ClientId = "OnlineBanking"; // Bu client��n �AuthServer�da ki Client_Id kar��l���.
        _.ClientSecret = "OnlineBanking"; // Bu client��n �AuthServer�da ki Client_Secret kar��l���.
        _.ResponseType = "code id_token"; // Olu�turulacak Authorization Code i�erisinde bulunmas�n� istedi�imiz datalar� belirtiyoruz. �code�, �retilecek olan authorize kodu ifade ederken,
                                          // �id_token� ise access token��n bize ait olan Auth Server�dan gelip gelmedi�ini test etmek i�in �retilen bir de�erdir.
        _.GetClaimsFromUserInfoEndpoint = true; // Ek taleplerin (claims) UserInfo Endpoint'inden al�naca��n� belirtir. Bu, kimlik belirtecinde (ID token) bulunmayan 
                                                // kullan�c� bilgilerini almak i�in kullan�l�r. (opsiyonel)
        _.SaveTokens = true; // Access Token ve Id Token'�n Elde edilmesini sa�lar (opsiyonel)
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
