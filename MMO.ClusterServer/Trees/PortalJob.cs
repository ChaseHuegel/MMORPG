using MMO.ClusterServer.Services;
using Swordfish.Library.BehaviorTrees;

namespace MMO.ClusterServer.Trees;

public class PortalJob : AbstractBehaviorJob<PortalService>
{
    public PortalJob(PortalService target) : base(target)
    {
    }

    protected override BehaviorTree<PortalService> TreeFactory()
    {
        return new BehaviorTree<PortalService>(
            new BehaviorDynamic<PortalService>(PollServerList)
        );
    }

    private BehaviorState PollServerList(PortalService service, float delta)
    {
        try
        {
            service.Poll();
            return BehaviorState.SUCCESS;
        }
        catch
        {
            return BehaviorState.FAILED;
        }
    }
}