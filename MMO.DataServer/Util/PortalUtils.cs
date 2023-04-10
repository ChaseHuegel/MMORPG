using System.Net.Http.Headers;
using System.Security.Authentication;

namespace MMO.DataServer.Util;

public class PortalRegistrationConfig
{
    public string User { get; set; }
    public string Password { get; set; }
    public string Hostname { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public PortalRegistrationConfig(
        string user,
        string password,
        string hostname,
        int port,
        string name,
        string type
    )
    {
        User = user;
        Password = password;
        Hostname = hostname;
        Port = port;
        Name = name;
        Type = type;
    }
}

public static class PortalUtils
{
    public static async Task RegisterWithPortalAsync(PortalRegistrationConfig config)
    {
        var host = new Swordfish.Library.Networking.Host
        {
            Hostname = config.Hostname,
            Port = config.Port
        };

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (
                request,
                certificate,
                chain,
                sslPolicyErrors
            ) => true
        };
        HttpClient httpClient = new(handler);

        //  Login to the portal
        var loginResult = await httpClient.PostAsync(
            $"https://localhost:7297/api/Session/Login?user={config.User}&password={config.Password}",
            null
        );
        if (!loginResult.IsSuccessStatusCode)
            throw new AuthenticationException();

        var jwtToken = await loginResult.Content.ReadAsStringAsync();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            jwtToken
        );

        //  Advertise this server via the portal
        await httpClient.PostAsync(
            $"https://localhost:7297/api/Servers?name={config.Name}&type={config.Type}&address={config.Hostname}",
            null
        );
    }
}
