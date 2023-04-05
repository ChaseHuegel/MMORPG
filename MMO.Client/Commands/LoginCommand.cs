using MMO.Bridge.Commands;
using MMO.Bridge.Types;
using MMO.Bridge.Util;
using MMO.Client.Services;
using Swordfish.Library.Networking;

namespace MMO.Client.Commands;

public class LoginCommand : Command
{
    public override string Option => "login";
    public override string Description => "Login to an account.";
    public override string ArgumentsHint => "<user> <password>";

    private readonly NetController _netController;
    private readonly PortalService _portalService;

    public LoginCommand(NetController netController, PortalService portalService)
    {
        _netController = netController;
        _portalService = portalService;
    }

    protected override async Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
    {
        string user = args.Take();
        string password = args.Take();

        string? token = await _portalService.TryLoginAsync(user, password);

        if (token == null)
            return CommandCompletion.Failure;

        var clusters = await _portalService.GetClustersAsync();

        var endPoint = clusters.First().Address.ParseIPEndPoint();
        _netController.Connect(endPoint, token);

        return CommandCompletion.Success;
    }
}