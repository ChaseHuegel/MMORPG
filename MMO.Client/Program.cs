using MMO.Bridge.Commands;
using MMO.Bridge.Packets;
using MMO.Client.Commands;
using MMO.Client.Services;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly<ChatPacket>();
PacketManager.RegisterAssembly();

var netClientSettings = new NetControllerSettings
{
    KeepAlive = TimeSpan.FromSeconds(10)
};
var netClient = new NetClient(netClientSettings);

var portalService = new PortalService("https://192.168.1.232:7297");

var commandParser = new CommandParser(
    indicator: '\0',
    new QuitCommand(),
    new ChatCommand(netClient),
    new LoginCommand(netClient, portalService)
);

while (true)
{
    string? input = Console.ReadLine();
    await commandParser.TryRunAsync(input);
}