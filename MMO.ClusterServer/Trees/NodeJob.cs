using MMO.Servers.Core;
using Swordfish.Library.BehaviorTrees;

namespace MMO.ClusterServer.Trees;

public class NodeJob : AbstractBehaviorJob<ServerNode>
{
    public NodeJob(ServerNode target) : base(target)
    {
    }

    protected override BehaviorTree<ServerNode> TreeFactory()
    {
        return new BehaviorTree<ServerNode>(
            new BehaviorSequence(
                new BehaviorSelector(
                    new BehaviorDynamic<ServerNode>(NodeIsStarted),
                    new BehaviorDynamic<ServerNode>(StartNode)
                ),
                new BehaviorSelector(
                    new BehaviorDynamic<ServerNode>(NodeIsRegistered),
                    new BehaviorDynamic<ServerNode>(RegisterNode)
                )
            )
        );
    }

    private BehaviorState NodeIsStarted(ServerNode service, float delta)
    {
        return service.Running ? BehaviorState.SUCCESS : BehaviorState.FAILED;
    }

    private BehaviorState StartNode(ServerNode service, float delta)
    {
        try
        {
            service.Start();
            return BehaviorState.SUCCESS;
        }
        catch
        {
            return BehaviorState.FAILED;
        }
    }

    private BehaviorState NodeIsRegistered(ServerNode node, float delta)
    {
        return node.Registered ? BehaviorState.SUCCESS : BehaviorState.FAILED;
    }

    private BehaviorState RegisterNode(ServerNode node, float delta)
    {
        try
        {
            node.RegisterWithPortalAsync().Wait();
            return BehaviorState.SUCCESS;
        }
        catch
        {
            return BehaviorState.FAILED;
        }
    }
}