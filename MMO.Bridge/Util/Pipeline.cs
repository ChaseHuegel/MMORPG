using Swordfish.Library.Collections;

namespace MMO.Bridge.Util;

public sealed class Pipeline : IPipelineService
{
    private readonly IPipelineService? _root;
    private readonly LockedList<IPipelineService> _requirements = new LockedList<IPipelineService>();

    public PipelineState State
    {
        get
        {
            //  Overall state is determined by the lowest state present in the pipeline.
            //  A given pipeline can't be complete until all of it's requirements are complete.
            PipelineState state = _root?.State ?? PipelineState.Complete;
            foreach (IPipelineService requirement in _requirements)
            {
                if (requirement.State < state)
                    state = requirement.State;
            }

            return state;
        }
    }

    private Pipeline(IPipelineService? root)
    {
        _root = root;
    }

    public Pipeline Requires(IPipelineService pipeline)
    {
        _requirements.Add(pipeline);
        return this;
    }

    public Pipeline Requires(params IPipelineService[] pipelines)
    {
        for (int i = 0; i < pipelines.Length; i++)
            Requires(pipelines[0]);

        return this;
    }

    public async Task StartAsync()
    {
        foreach (IPipelineService requirement in _requirements)
            await requirement.StartAsync();

        if (_root != null)
            await _root.StartAsync();
    }

    public static Pipeline Create(IPipelineService? root = null)
    {
        return new Pipeline(root);
    }
}

public interface IPipelineService
{
    Task StartAsync();

    PipelineState State { get; }
}

public enum PipelineState
{
    Unstarted,
    Active,
    Complete
}