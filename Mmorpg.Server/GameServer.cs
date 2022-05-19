using System.Reflection;
using System.Collections.Concurrent;
using System.Net;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Util;
using Mmorpg.Data;
using Mmorpg.Packets;

namespace MMORPG.Server
{
    public class GameServer : NetServer
    {
        private static ServerConfig s_Configuration;
        public static ServerConfig Configuration => s_Configuration ?? (s_Configuration = Config.Load<ServerConfig>("config/server.toml"));

        public static GameServer Instance;

        public ConcurrentDictionary<int, LivingEntity> Characters = new ConcurrentDictionary<int, LivingEntity>();

        public Dictionary<EndPoint, string> Logins = new Dictionary<EndPoint, string>();

        public GameServer() : base(Configuration.Connection.Port)
        {
            Instance = this;
            MaxSessions = Configuration.Connection.MaxPlayers;
            
            HandshakePacket.ValidateHandshakeCallback = ValidateHandshake;
            HandshakePacket.ValidationSignature = "Ekahsdnah";
        }

        public bool ValidateHandshake(EndPoint endPoint)
        {
            return Logins.ContainsKey(endPoint);
        }

        public override void OnSessionStarted(object sender, NetEventArgs e)
        {
            LivingEntity character = new LivingEntity {
                ID = e.Session.ID
            };

            //  Send a snapshot of the new player to all other players
            EntityPacket snapshot = new EntityPacket {
                ID = character.ID,
                X = character.X,
                Y = character.Y,
                Z = character.Z,
                Heading = character.Heading,
                Speed = character.Speed,
                Direction = character.Direction,
                State = {
                    [0] = character.Jumped,
                    [1] = character.Moving
                }
            };
            BroadcastExcept(snapshot, e.Session);

            //  Send a snapshot of all entities to the new player
            foreach (LivingEntity entity in Characters.Values)
            {                
                Send(new EntityPacket {
                    ID = entity.ID,
                    X = entity.X,
                    Y = entity.Y,
                    Z = entity.Z,
                    Heading = entity.Heading,
                    Speed = entity.Speed,
                    Direction = entity.Direction,
                    State = {
                        [0] = entity.Jumped,
                        [1] = entity.Moving
                    }
                }, e.Session);
            }

            //  Add the new player to the world
            Characters.TryAdd(e.Session.ID, character);
            Console.WriteLine($"[{e.Session}] joined the game world.");
        }
    }
}
