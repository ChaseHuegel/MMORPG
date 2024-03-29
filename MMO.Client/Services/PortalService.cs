using System.Net.Http.Json;
using System.Security.Authentication;
using MMO.Bridge.Models;

namespace MMO.Client.Services
{
    public class PortalService
    {
        private readonly Uri BaseUri;

        public PortalService(string address)
        {
            BaseUri = new Uri(address);
        }

        public async Task<string?> TryLoginAsync(string user, string password)
        {
            var uri = new Uri(BaseUri, $"/api/Session/Login?user={user}&password={password}");
            var loginResult = await HttpClientFactory().PostAsync(uri, null);

            if (!loginResult.IsSuccessStatusCode)
                throw new AuthenticationException();

            var jwtToken = await loginResult.Content.ReadAsStringAsync();
            return jwtToken;
        }

        public async Task<Server[]> GetServersAsync()
        {
            var uri = new Uri(BaseUri, "/api/Servers");
            Server[]? servers = await HttpClientFactory().GetFromJsonAsync<Server[]>(uri);
            return servers ?? Array.Empty<Server>();
        }

        public async Task<Server[]> GetClustersAsync()
        {
            Server[]? servers = await GetServersAsync();
            return servers.Where(server => server.Type.Equals("Cluster")).ToArray();
        }

        private static HttpClient HttpClientFactory()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (
                    request,
                    certificate,
                    chain,
                    sslPolicyErrors
                ) => true
            };
            var client = new HttpClient(handler);
            return client;
        }
    }
}
