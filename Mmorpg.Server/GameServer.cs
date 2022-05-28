using System.Collections.Concurrent;
using System.Net;

using Mmorpg.Data;
using Mmorpg.Server.Control;
using Mmorpg.Server.Data;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Util;

namespace Mmorpg.Server
{
    public class GameServer : NetServer
    {
        private static ServerConfig s_Configuration;
        public static ServerConfig Configuration => s_Configuration ?? (s_Configuration = Config.Load<ServerConfig>("config/server.toml"));

        public static GameServer Instance;
        public WorldView WorldView;

        public ConcurrentDictionary<EndPoint, Character> Players = new ConcurrentDictionary<EndPoint, Character>();
        public ConcurrentDictionary<EndPoint, string> Logins = new ConcurrentDictionary<EndPoint, string>();

        public GameServer() : base(Configuration.Connection.Port)
        {
            Instance = this;
            MaxSessions = Configuration.Connection.MaxPlayers;
            
            HandshakePacket.ValidateHandshakeCallback = ValidateHandshake;
            HandshakePacket.ValidationSignature = "Ekahsdnah";

            WorldView = new WorldView(this);
        }

        public void Tick(float deltaTime)
        {
            WorldView.Tick(deltaTime);
        }

        public bool ValidateHandshake(EndPoint endPoint)
        {
            return Logins.ContainsKey(endPoint);
        }

        public override void OnSessionStarted(object sender, NetEventArgs e)
        {
            Players.TryGetValue(e.EndPoint, out Character character);
            WorldView.AddPlayer(character, e.Session);

            Console.WriteLine($"[{e.Session}] joined the game world as [{character.Name}].");
        }
    }
}
