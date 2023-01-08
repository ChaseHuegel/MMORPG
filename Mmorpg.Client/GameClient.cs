using System.Collections.Concurrent;
using Mmorpg.Data;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Packets;

namespace Mmorpg.Client
{
    public class GameClient : NetClient
    {
        public static GameClient Instance;

        public ConcurrentDictionary<int, LivingEntity> Characters;

        public GameClient(NetControllerSettings settings) : base(settings)
        {
            Instance = this;
            Characters = new ConcurrentDictionary<int, LivingEntity>();

            HandshakePacket.ValidationSignature = "Ekahsdnah";
        }
    }
}
