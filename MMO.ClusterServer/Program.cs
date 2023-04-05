using System.Net.Http.Json;
using MMO.Bridge.Models;
using MMO.Bridge.Packets;
using MMO.Bridge.Util;
using MMO.Servers.Core;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly();
PacketManager.RegisterAssembly<ChatPacket>();

var node = new ServerNode();
await node.StartAsync(args);

//  Collect known servers from the portal
var httpClient = new HttpClient();
Server[]? servers = await httpClient.GetFromJsonAsync<Server[]>("https://localhost:7297/api/Servers");

//  Find a chat server and connect to it
var chatServer = servers!.First(server => server.Type.Equals("Chat"));
var chatServerEndPoint = chatServer.Address.ParseIPEndPoint();
node.Connect(chatServerEndPoint);

//  Setup packet routing
node.AddPacketRoute<ChatPacket>(chatServerEndPoint);

while (node.Running)
{
    //  Keep alive
}