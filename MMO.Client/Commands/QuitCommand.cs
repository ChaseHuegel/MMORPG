using MMO.Bridge.Commands;
using MMO.Bridge.Types;

namespace MMO.Client.Commands;

public class QuitCommand : Command
{
    public override string Option => "quit";
    public override string Description => "Exits the application.";
    public override string ArgumentsHint => "";

    protected override Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
    {
        Environment.Exit(0);
        return Task.FromResult(CommandCompletion.Success);
    }
}