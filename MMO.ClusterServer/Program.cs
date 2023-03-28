using System.Net.Http.Json;
using MMO.Bridge.Packets;
using MMO.Servers.Core;
using MMO.Servers.Core.Models;
using MMO.Servers.Core.Util;
using Swordfish.Library.Networking;

PacketManager.RegisterAssembly();

var node = new ServerNode();
node.Start(args);

Thread.Sleep(1000);

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