using System.Security.Claims;

namespace MMO.Portal.Util;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserClaim(this ClaimsPrincipal principal)
    {
        return principal.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
            ?.Value;
    }
}
