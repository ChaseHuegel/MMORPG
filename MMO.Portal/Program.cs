using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MMO.Portal.Controllers;
using MMO.Portal.Models;

var builder = WebApplication.CreateBuilder(args);
var portalConnectionString = builder.Configuration.GetConnectionString("portal");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<PortalDbContext>(
    options => options.UseMySql(portalConnectionString, ServerVersion.AutoDetect(portalConnectionString))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/api/Session/Login";
        options.LogoutPath = "/api/Session/Logout";
        options.Cookie.Name = "MMO.Portal.Login";
    });

builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(portalConnectionString));
builder.Services.AddScoped<UserManager>();

builder.Services.AddSingleton<ServerManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
