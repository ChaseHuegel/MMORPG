using System.Collections.Concurrent;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Util;
using Mmorpg.Data;

namespace Mmorpg.Client
{
    public class GameClient : NetClient
    {
        private static ClientConfig s_Configuration;
        public static ClientConfig Configuration => s_Configuration ?? (s_Configuration = Config.Load<ClientConfig>("config/client.toml"));

        public static GameClient Instance;

        public ConcurrentDictionary<int, LivingEntity> Characters;

        public GameClient() : base(Configuration.Connection.Host) {
            Instance = this;
            Characters = new ConcurrentDictionary<int, LivingEntity>();
            
            HandshakePacket.ValidationSignature = "Ekahsdnah";
        }
    }
}
