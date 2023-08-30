using MMO.Bridge.Util;
using MMO.Servers.Core;

namespace MMO.ClusterServer.Services;

public class NodeService : ConnectionHandler
{
    private ServerNode _node;
    private string[] _args;

    public NodeService(ServerNode node, string[] args)
    {
        _node = node;
        _args = args;
    }

    public override bool IsConnected()
    {
        return _node.Running;
    }

    protected override void Disconnect()
    {
        throw new NotSupportedException();
    }

    protected override Task TryConnectAsync()
    {
        return _node.StartAsync(_args);
    }
}