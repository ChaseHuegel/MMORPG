using Swordfish.Extensibility;
using Swordfish.Graphics;
using Swordfish.Library.IO;
using MMO.Client.Commands;
using MMO.Client.Models;
using MMO.Client.Services;
using Swordfish.Library.Networking;
using Swordfish.Library.Util;

namespace MMO.Client.Control;

public class ClientController : Plugin
{
    public override string Name => "Client Controller";
    public override string Description => "A core implementation of high-level functionality.";

    public readonly CommandParser CommandParser;

    private readonly IWindowContext WindowContext;
    private readonly IShortcutService ShortcutService;

    public ClientController(IWindowContext windowContext, IShortcutService shortcutService)
    {
        WindowContext = windowContext;
        ShortcutService = shortcutService;

        ClientConfig config = Config.Load<ClientConfig>("config/client.toml");

        var netClientSettings = new NetControllerSettings { KeepAlive = TimeSpan.FromSeconds(10) };
        var netClient = new NetClient(netClientSettings);

        var portalService = new PortalService(config.Connection.PortalUrl);

        CommandParser = new CommandParser(
            indicator: '/',
            new QuitCommand(this, netClient),
            new ChatCommand(netClient),
            new LoginCommand(netClient, portalService)
        );

        //  TODO There is a bug in the engine here. Maximize can rarely be missed by running in the ctor instead of Start.
        WindowContext.Maximize();

        ShortcutService.RegisterShortcut(
            new Shortcut(
                "Exit",
                "General",
                ShortcutModifiers.NONE,
                Key.ESC,
                Shortcut.DefaultEnabled,
                Exit
            )
        );
    }

    public override void Start()
    {
    }

    public void Exit()
    {
        WindowContext.Close();
    }
}