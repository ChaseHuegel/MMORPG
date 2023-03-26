using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MMO.Portal.Models;

namespace MMO.Portal.Controllers;

public class UserManager
{
    private readonly PortalDbContext _dbContext;

    public UserManager(PortalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SignInAsync(HttpContext httpContext, Account account, bool isPersistent = false)
    {
        var identity = new ClaimsIdentity(GetUserClaims(account), CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
        });
    }

    public async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync();
    }

    private static IEnumerable<Claim> GetUserClaims(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.User),
            new Claim(ClaimTypes.Email, account.Email)
        };

        claims.AddRange(GetUserRoleClaims(account));
        return claims;
    }

    private static IEnumerable<Claim> GetUserRoleClaims(Account user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.User),
        };

        if (!string.IsNullOrWhiteSpace(user.Roles))
            claims.Add(new Claim(ClaimTypes.Role, user.Roles));

        return claims;
    }
}