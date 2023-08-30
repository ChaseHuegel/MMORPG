using MMO.Bridge.Models;
using MMO.Bridge.Packets;
using MMO.ClusterServer.Services;
using MMO.Servers.Core;
using MMO.Services.ClusterServer;
using Swordfish.Library.BehaviorTrees;

namespace MMO.ClusterServer.Trees;

public class ClusterPipeline
{
    public BehaviorPipeline Pipeline { get; }

    public ClusterPipeline(PortalService portalService)
    {
        ServerNode clusterNode = new();

        ServerRoute chatRoute = new(
            clusterNode,
            "Chat",
            new[] {
                typeof(ChatPacket)
            }
        );

        IBehaviorJob portalJob = new PortalJob(portalService);
        IBehaviorJob clusterJob = new NodeJob(clusterNode);
        IBehaviorJob chatRouteJob = new ServerRouteJob(chatRoute, portalService);

        Pipeline = new BehaviorPipeline(portalJob, clusterJob, chatRouteJob);
    }

    public void Run()
    {
        while (Pipeline.Tick(0f) != BehaviorState.SUCCESS)
        {
            //  Keep ticking
        }
    }
}