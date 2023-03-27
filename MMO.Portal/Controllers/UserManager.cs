using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MMO.Portal.Models;

namespace MMO.Portal.Controllers;

public class UserManager
{
    private readonly PortalDbContext _dbContext;

    private static readonly ConcurrentDictionary<string, Account> LoggedInUsers = new();

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

        LoggedInUsers.TryAdd(account.User, account);
    }

    public async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync();
        string userClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        LoggedInUsers.Remove(userClaim, out _);
    }

    public bool IsUserLoggedIn(string user)
    {
        return LoggedInUsers.ContainsKey(user);
    }

    public bool IsUserLoggedIn(ClaimsPrincipal principal)
    {
        string userClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return IsUserLoggedIn(userClaim);
    }

    private static IEnumerable<Claim> GetUserClaims(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.User),
        };

        if (!string.IsNullOrWhiteSpace(account.Roles))
            claims.AddRange(account.Roles.Split(',').Select(x => new Claim(ClaimTypes.Role, x)));

        return claims;
    }
}