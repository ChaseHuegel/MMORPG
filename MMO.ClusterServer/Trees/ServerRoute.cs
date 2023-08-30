using System.Net;
using MMO.Servers.Core;

namespace MMO.ClusterServer.Trees;

public class ServerRoute
{
    public ServerNode ServerNode { get; }

    public string Type { get; }

    public Type[] PacketTypes { get; }

    public IPEndPoint? RouteEndPoint { get; set; }

    public ServerRoute(ServerNode serverNode, string type, Type[] packetTypes)
    {
        ServerNode = serverNode;
        Type = type;
        PacketTypes = packetTypes;
        RouteEndPoint = null;
    }
}