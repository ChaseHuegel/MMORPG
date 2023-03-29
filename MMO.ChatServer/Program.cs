using MMO.Servers.Core;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly();

var node = new ServerNode();
await node.StartAsync(args);

while (node.Running)
{
    //  Keep alive
}
