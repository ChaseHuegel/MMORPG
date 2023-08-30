using System.Net.Http.Json;
using MMO.Bridge.Models;
using MMO.Bridge.Util;

namespace MMO.ClusterServer.Services;

public class PortalService : ConnectionHandler
{
    private Server[]? _servers;
    private readonly Uri _serversEndpoint;

    public PortalService(Uri host)
    {
        _serversEndpoint = new Uri(host, "api/Servers");
    }

    public async Task<Server> GetServerAsync(string type)
    {
        Server match;
        while (!TryFindServer(type, out match))
        {
            await ReconnectAsync();
        }

        bool TryFindServer(string value, out Server server)
        {
            Server? match = _servers?.FirstOrDefault(server => server.Type.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            server = match!.Value;
            return match.HasValue && match.Value.Address != null;
        }

        return match;
    }

    public override bool IsConnected()
    {
        return _servers != null && _servers.Length > 0;
    }

    protected override async Task TryConnectAsync()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) =>
                true
        };

        var httpClient = new HttpClient(handler);
        _servers = await httpClient.GetFromJsonAsync<Server[]>(_serversEndpoint);
    }

    protected override void Disconnect()
    {
        _servers = null;
    }
}