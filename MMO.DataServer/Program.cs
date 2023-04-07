using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MMO.DataServer.Data;
using MMO.DataServer.Handlers;

var builder = WebApplication.CreateBuilder(args);
var portalConnectionString = builder.Configuration.GetConnectionString("portal");

var authServerUrl = builder.Configuration.GetValue<string>("authenticationServerUrl");
var authServerCookie = builder.Configuration.GetValue<string>("authenticationCookie");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<CharactersDbContext>(
    options => options.UseMySql(portalConnectionString, ServerVersion.AutoDetect(portalConnectionString))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = "/api/Session/Login";
            options.LogoutPath = "/api/Session/Logout";
            options.Cookie.Name = "MMO.DataServer.Login";
        }
    )
    .AddRemoteScheme<RemoteAuthenticationOptions, PortalAuthenticationHandler>("portal", "PortalRemoteAuth", RemoteAuthenticationOptionsFactory);

void RemoteAuthenticationOptionsFactory(RemoteAuthenticationOptions options)
{
    options.CallbackPath = "/api/Session/Login";
}

var app = builder.Build();

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
