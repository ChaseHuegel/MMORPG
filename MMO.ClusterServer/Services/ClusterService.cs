using MMO.Bridge.Models;
using MMO.Bridge.Packets;
using MMO.Bridge.Util;
using MMO.ClusterServer.Services;
using MMO.Servers.Core;

namespace MMO.Services.ClusterServer;

public class ClusterService
{
    private readonly ServerNode _serverNode;
    private readonly PortalService _portalService;

    private bool _connected;
    private bool _chatServerIdentified;

    public ClusterService(ServerNode serverNode, PortalService portalService)
    {
        _serverNode = serverNode;
        _portalService = portalService;
    }

    private async Task<bool> ConnectChatServer()
    {
        //  TODO just because we've identified a chat server doesn't mean we will connect to it.
        //  TODO we've got to handle the situation where we need to connect to a different or new chat server.
        if (_chatServerIdentified)
            return true;

        //  As-is this doesn't handle recovery or scaling. We're naively picking the first server and assuming we'll be able to connect.
        Server chatServer = await _portalService.GetServerAsync("Chat");
        var chatServerEndPoint = chatServer.Address.ParseIPEndPoint();
        _chatServerIdentified = true;

        //  TODO this is weak. We need to be able to confirm Connect succeeded.
        _serverNode.Connect(chatServerEndPoint);

        //  TODO need to be able to reset or update routes. Once we've picked a chat server, we can't change which one we're using.
        _serverNode.AddPacketRoute<ChatPacket>(chatServerEndPoint);

        return true;
    }
}