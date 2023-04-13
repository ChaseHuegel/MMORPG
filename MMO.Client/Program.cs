using MMO.Bridge.Commands;
using MMO.Bridge.Packets;
using MMO.Client;
using MMO.Client.Commands;
using MMO.Client.Models;
using MMO.Client.Services;
using Swordfish.Library.Networking;
using Swordfish.Library.Util;

PacketManager.RegisterAssembly<ChatPacket>();
PacketManager.RegisterAssembly();

ClientConfig config = Config.Load<ClientConfig>("config/client.toml");

var application = new Application();

var netClientSettings = new NetControllerSettings { KeepAlive = TimeSpan.FromSeconds(10) };
var netClient = new NetClient(netClientSettings);

var portalService = new PortalService(config.Connection.PortalUrl);

var commandParser = new CommandParser(
    indicator: '\0',
    new QuitCommand(application, netClient),
    new ChatCommand(netClient),
    new LoginCommand(netClient, portalService)
);

application.Update += OnUpdate;
async void OnUpdate(object? sender, EventArgs e)
{
    string? input = Console.ReadLine();
    _ = await commandParser.TryRunAsync(input);
}

return await application.RunAsync();
