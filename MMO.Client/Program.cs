using MMO.Bridge.Packets;
using MMO.Client;
using MMO.Client.Models;
using MMO.Client.Services;
using Swordfish;
using Swordfish.Library.Networking;
using Swordfish.Library.Util;

PacketManager.RegisterAssembly<ChatPacket>();
PacketManager.RegisterAssembly();

var engine = new SwordfishEngine();
return engine.Run(args);