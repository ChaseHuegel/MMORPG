using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace MMO.DataServer.Handlers;

public class PortalAuthenticationHandler : RemoteAuthenticationHandler<RemoteAuthenticationOptions>
{
    public PortalAuthenticationHandler(IOptionsMonitor<RemoteAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        Context.Request.Cookies.TryGetValue("MMO.Portal.Login", out string cookie);

        if (cookie == null)
            return HandleRequestResult.Fail("Portal authentication cookie not found.");

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) => true
        };
        var httpClient = new HttpClient(handler);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "MMO.Portal.Login=" + cookie);
        var validateResponse = await httpClient.PostAsync("https://localhost:7297/api/Session/Validate", null);

        if (!validateResponse.IsSuccessStatusCode)
            return HandleRequestResult.Fail(new UnauthorizedAccessException(validateResponse.StatusCode.ToString()));

        var claims = new Claim[] {
            new Claim(ClaimTypes.NameIdentifier, "test")
        };
        var identity = new ClaimsIdentity(claims, "portal");
        var principal = new ClaimsPrincipal(identity);

        return HandleRequestResult.Success(new AuthenticationTicket(principal, "portal"));
    }
}