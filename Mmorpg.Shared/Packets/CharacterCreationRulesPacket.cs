using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = false)]
    public struct CharacterCreationRulesPacket : ISerializedPacket
    {
        public string[] RaceNames;

        public string[] ClassNames;

        public int[] RaceClassMask;
    }
}
