using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MMO.Portal.Models;

namespace MMO.Portal.Managers;

public class UserManager
{
    private readonly ConfigurationManager _configuration;

    private static readonly ConcurrentDictionary<string, Account> LoggedInUsers = new();

    public UserManager(ConfigurationManager configuration)
    {
        _configuration = configuration;
    }

    public Task<string> SignInAsync(Account account)
    {
        var identity = new ClaimsIdentity(GetUserClaims(account), JwtBearerDefaults.AuthenticationScheme);

        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Key"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration.GetValue<string>("JWT:Issuer"),
            Audience = _configuration.GetValue<string>("JWT:Audience"),
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        LoggedInUsers.TryAdd(account.User, account);

        return Task.FromResult(tokenString);
    }

    public Task SignOutAsync(Account account)
    {
        LoggedInUsers.Remove(account.User, out _);
        return Task.CompletedTask;
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