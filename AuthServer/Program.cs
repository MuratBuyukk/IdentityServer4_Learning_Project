using AuthServer;
using AuthServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

// Identity Server Congfiguration
builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddTestUsers(Config.GetTestUsers().ToList())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddDeveloperSigningCredential()
    .AddProfileService<CustomProfileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();
