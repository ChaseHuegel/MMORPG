using System.Net;
using System.Net.Sockets;
using MMO.ChatServer.Models;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Handlers;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Util;

namespace MMO.ChatServer;

internal class Program
{
    private static void Main(string[] args)
    {
        ServerConfig config = Config.Load<ServerConfig>("config/server.toml");

        InitializeNetController(config);

        RegisterWithPortal(config);

        while (true)
        {
            //  Keep running
        }
    }

    private static void InitializeNetController(ServerConfig config)
    {
        PacketManager.RegisterAssembly();

        var host = new Host
        {
            Hostname = config.Connection.Address
        };

        NetControllerSettings netControllerSettings = new()
        {
            AddressFamily = AddressFamily.InterNetwork,
            Port = config.Connection.Port,
            MaxSessions = config.Connection.MaxSessions,
            SessionExpiration = config.Connection.SessionExpiration,
            Address = host.Address
        };

        HandshakeHandler.ValidateHandshakeCallback = ValidateHandshake;
        HandshakePacket.ValidationSignature = "Ekahsdnah";

        var netController = new NetController(netControllerSettings);
        netController.SessionStarted += OnSessionStarted;
        netController.SessionEnded += OnSessionEnded;
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

    public static void OnSessionStarted(object? sender, NetEventArgs e)
    {
        Console.WriteLine($"[{e.Session}] joined.");
    }

    public static void OnSessionEnded(object? sender, NetEventArgs e)
    {
        Console.WriteLine($"[{e.Session}] disconnected.");
    }
}