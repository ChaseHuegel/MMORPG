using MMO.Servers.Core;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly();

var node = new ServerNode();
node.Start(args);

while (node.Running)
{
    //  Keep alive
}
