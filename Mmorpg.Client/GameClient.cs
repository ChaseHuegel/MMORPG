using System.Collections.Concurrent;
using Mmorpg.Data;
using Mmorpg.Shared.Packets;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Util;

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
