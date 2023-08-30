using MMO.Bridge.Packets;
using MMO.Bridge.Util;
using MMO.ClusterServer.Services;
using MMO.Servers.Core;
using MMO.Services.ClusterServer;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly();
PacketManager.RegisterAssembly<ChatPacket>();

var portalUri = new Uri("https://localhost:7297");
var portalService = new PortalService(portalUri);

var node = new ServerNode();
var nodeService = new NodeService(node, args);

var clusterService = new ClusterService(node, portalService);

var nodePipeline = Pipeline.Create(nodeService).Requires(portalService);
var clusterPipeline = Pipeline.Create(clusterService).Requires(nodePipeline);
await clusterPipeline.StartAsync();

while (clusterPipeline.State != PipelineState.Complete)
{
    //  Keep alive
}
