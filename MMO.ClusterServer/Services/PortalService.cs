using System.Net.Http.Json;
using MMO.Bridge.Models;
using MMO.Bridge.Util;

namespace MMO.ClusterServer.Services;

public class PortalService
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
        }

        bool TryFindServer(string value, out Server server)
        {
            Server? match = _servers?.FirstOrDefault(server => server.Type.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            server = match!.Value;
            return match.HasValue && match.Value.Address != null;
        }

        return match;
    }

    public bool TryGetServer(string type, out Server server)
    {
        server = default;
        if (_servers == null || _servers.Length == 0)
            return false;

        for (int i = 0; i < _servers.Length; i++)
        {
            server = _servers[i];
            if (server.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase))
                return true;
        }

        return false;
    }

    public void Poll()
    {
        PollAsync().Wait();
    }

    public async Task PollAsync()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) =>
                true
        };

        var httpClient = new HttpClient(handler);
        _servers = await httpClient.GetFromJsonAsync<Server[]>(_serversEndpoint);
    }
}