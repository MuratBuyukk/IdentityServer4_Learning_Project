using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// JWT authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:1000"; // Auth Server URL
        options.Audience = "Garanti"; // Resource name in Auth Server
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadGaranti", policy => policy.RequireClaim("scope", "Garanti.Read"));
    options.AddPolicy("WriteGaranti", policy => policy.RequireClaim("scope", "Garanti.Write"));
    options.AddPolicy("ReadWriteGaranti", policy => policy.RequireClaim("scope", "Garanti.Write", "Garanti.Read"));
    options.AddPolicy("AllGaranti", policy => policy.RequireClaim("scope", "Garanti.Admin"));
});




var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
