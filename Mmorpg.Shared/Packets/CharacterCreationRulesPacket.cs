using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = false)]
    public class CharacterCreationRulesPacket : Packet
    {
        public string[] RaceNames;

        public string[] ClassNames;

        public int[] RaceClassMask;
    }
}
