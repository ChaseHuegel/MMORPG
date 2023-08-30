using MMO.Bridge.Models;
using MMO.Bridge.Util;
using MMO.ClusterServer.Services;
using Swordfish.Library.BehaviorTrees;

namespace MMO.ClusterServer.Trees;

public class ServerRouteJob : AbstractBehaviorJob<ServerRoute>
{
    private readonly PortalService _portalService;

    public ServerRouteJob(ServerRoute target, PortalService portalService) : base(target)
    {
        _portalService = portalService;
    }

    protected override BehaviorTree<ServerRoute> TreeFactory()
    {
        return new BehaviorTree<ServerRoute>(
            new BehaviorSelector(
                new BehaviorDynamic<ServerRoute>(RouteIsConnected),
                new BehaviorSequence(
                    new BehaviorDynamic<ServerRoute>(RequestRoute),
                    new BehaviorDynamic<ServerRoute>(ConnectToRoute),
                    new BehaviorDynamic<ServerRoute>(SetupRoutes),
                    new BehaviorDynamic<ServerRoute>(AttachDisconnectListener)
                )
            )
        );
    }

    private BehaviorState RouteIsConnected(ServerRoute route, float arg2)
    {
        return route.RouteEndPoint != null ? BehaviorState.SUCCESS : BehaviorState.FAILED;
    }

    private BehaviorState RequestRoute(ServerRoute route, float arg2)
    {
        if (_portalService.TryGetServer(route.Type, out Server server))
        {
            route.RouteEndPoint = server.Address.ParseIPEndPoint();
            return BehaviorState.SUCCESS;
        }

        return BehaviorState.FAILED;
    }

    private BehaviorState ConnectToRoute(ServerRoute route, float arg2)
    {
        if (route.RouteEndPoint == null)
            return BehaviorState.FAILED;

        try
        {
            route.ServerNode.Connect(route.RouteEndPoint);
            return BehaviorState.SUCCESS;
        }
        catch
        {
            return BehaviorState.FAILED;
        }
    }

    private BehaviorState SetupRoutes(ServerRoute route, float arg2)
    {
        if (route.RouteEndPoint == null)
            return BehaviorState.FAILED;

        for (int i = 0; i < route.PacketTypes.Length; i++)
            route.ServerNode.AddPacketRoute(route.PacketTypes[i], route.RouteEndPoint);

        return BehaviorState.SUCCESS;
    }

    private BehaviorState AttachDisconnectListener(ServerRoute route, float arg2)
    {
        //  TODO on disconnect we have to re-evaluate the tree
        return BehaviorState.SUCCESS;
    }
}