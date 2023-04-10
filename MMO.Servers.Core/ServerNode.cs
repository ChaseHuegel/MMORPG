using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Authentication;
using MMO.Servers.Core.Models;
using Swordfish.Library.Diagnostics;
using Swordfish.Library.Networking;
using Swordfish.Library.Util;

namespace MMO.Servers.Core;

public class ServerNode
{
    public bool Running { get; private set; }

    private NetServer NetServer;

    private string? JwtToken;

    private readonly Dictionary<Type, IPEndPoint> PacketRoutes = new();

    public async Task StartAsync(string[] args)
    {
        ServerConfig config = Config.Load<ServerConfig>("config/server.toml");

        InitializeNetController(config);
        await RegisterWithPortalAsync(config);

        Running = true;
    }

    public void Connect(IPEndPoint endPoint)
    {
        NetServer.Connect(endPoint, JwtToken);
    }

    public void AddPacketRoute<TPacket>(IPEndPoint endPoint) where TPacket : Packet
    {
        PacketRoutes.Add(typeof(TPacket), endPoint);
    }

    private void InitializeNetController(ServerConfig config)
    {
        PacketManager.RegisterAssembly();

        NetControllerSettings netControllerSettings = new()
        {
            AddressFamily = AddressFamily.InterNetwork,
            Port = config.Connection.Port,
            MaxSessions = config.Connection.MaxSessions,
            SessionExpiration = config.Connection.SessionExpiration,
            KeepAlive = TimeSpan.FromSeconds(10),
        };

        NetServer = new NetServer(netControllerSettings);
        NetServer.PacketAccepted += OnPacketAccepted;
        NetServer.HandshakeValidateCallback = HandshakeValidateCallback;
    }

    private async Task RegisterWithPortalAsync(ServerConfig config)
    {
        var host = new Host
        {
            Hostname = config.Connection.Address,
            Port = config.Connection.Port
        };

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) => true
        };
        HttpClient httpClient = new(handler);

        //  Login to the portal
        var loginResult = await httpClient.PostAsync($"https://localhost:7297/api/Session/Login?user={config.Authentication.User}&password={config.Authentication.Password}", null);
        if (!loginResult.IsSuccessStatusCode)
            throw new AuthenticationException();

        JwtToken = await loginResult.Content.ReadAsStringAsync();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);

        //  Advertise this server via the portal
        await httpClient.PostAsync($"https://localhost:7297/api/Servers?name={config.Registration.Name}&type={config.Registration.Type}&address={host}", null);
    }

    private bool HandshakeValidateCallback(EndPoint endPoint, string secret)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) => true
        };
        HttpClient httpClient = new(handler);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secret);

        var task = httpClient.PostAsync($"https://localhost:7297/api/Session/Validate", null);
        task.Wait();
        var loginResult = task.Result;

        return loginResult.IsSuccessStatusCode;
    }

    private void OnPacketAccepted(object? sender, NetEventArgs e)
    {
        var packetType = e.Packet.GetType();

        if (PacketRoutes.TryGetValue(packetType, out IPEndPoint? routeEndPoint))
        {
            //  If a route sent the packet, forward it to everyone else.
            if (e.EndPoint.Equals(routeEndPoint))
            {
                Debugger.Log($"Routing packet from {e.Session} to clients.");
                NetServer.BroadcastExcept(e.Packet, e.Session);
            }
            //  Else forward it to the route.
            else
            {
                Debugger.Log($"Routing packet from {e.Session} to server {routeEndPoint}.");
                NetServer.Send(e.Packet, routeEndPoint);
            }
        }
    }
}