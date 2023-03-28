using System.Net;
using System.Net.Sockets;
using MMO.Servers.Core.Models;
using Swordfish.Library.Diagnostics;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Handlers;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Util;

namespace MMO.Servers.Core;

public class ServerNode
{
    public bool Running { get; private set; }

    private NetServer NetServer;

    private readonly Dictionary<Type, IPEndPoint> PacketRoutes = new();

    public void Start(string[] args)
    {
        ServerConfig config = Config.Load<ServerConfig>("config/server.toml");

        InitializeNetController(config);
        RegisterWithPortal(config);

        Running = true;
    }

    public void Connect(IPEndPoint endPoint)
    {
        NetServer.Send(HandshakePacket.New, endPoint);
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
            SessionExpiration = config.Connection.SessionExpiration
        };

        HandshakeHandler.ValidateHandshakeCallback = ValidateHandshake;
        HandshakePacket.ValidationSignature = "Ekahsdnah";

        NetServer = new NetServer(netControllerSettings);
        NetServer.PacketAccepted += OnPacketAccepted;
    }

    private static void RegisterWithPortal(ServerConfig config)
    {
        var host = new Host
        {
            Hostname = config.Connection.Address,
            Port = config.Connection.Port
        };

        HttpClient httpClient = new();
        httpClient.PostAsync($"https://localhost:7297/api/Session/Login?user={config.Authentication.User}&password={config.Authentication.Password}", null).Wait();
        httpClient.PostAsync($"https://localhost:7297/api/Servers?name={config.Registration.Name}&type={config.Registration.Type}&address={host}", null).Wait();
    }

    private static bool ValidateHandshake(EndPoint endPoint)
    {
        //  TODO validate the endpoint has an associated login
        return true;
    }

    private void OnPacketAccepted(object? sender, NetEventArgs e)
    {
        var packetType = e.Packet.GetType();

        if (PacketRoutes.TryGetValue(packetType, out IPEndPoint? routeEndPoint))
        {
            //  If a route sent the packet, forward it to everyone else.
            if (e.EndPoint == routeEndPoint)
            {
                NetServer.BroadcastExcept(e.Packet, e.Session);
            }
            //  Else forward it to the route.
            else
            {
                NetServer.Send(e.Packet, routeEndPoint);
            }
        }
    }
}