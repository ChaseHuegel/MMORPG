using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MMO.Portal.Controllers;
using MMO.Portal.Models;

var builder = WebApplication.CreateBuilder(args);
var portalConnectionString = builder.Configuration.GetConnectionString("portal");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<PortalDbContext>(
    options => options.UseMySql(portalConnectionString, ServerVersion.AutoDetect(portalConnectionString))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/api/Accounts/Login";
        options.LogoutPath = "/api/Accounts/Logout";
    });

builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddTransient<UserManager>();

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(portalConnectionString));
// builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>();

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
