using MMO.Bridge.Packets;
using MMO.ClusterServer.Services;
using MMO.ClusterServer.Trees;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly();
PacketManager.RegisterAssembly<ChatPacket>();

var portalUri = new Uri("https://localhost:7297");
var portalService = new PortalService(portalUri);

var clusterPipeline = new ClusterPipeline(portalService);
clusterPipeline.Run();

while (true)
{
    //  Keep alive
}
