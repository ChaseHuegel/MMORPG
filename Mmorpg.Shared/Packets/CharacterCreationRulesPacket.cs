using Needlefish;

using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = false)]
    public struct CharacterCreationRulesPacket : IDataBody
    {
        public string[] RaceNames;

        public string[] ClassNames;

        public int[] RaceClassMask;
    }
}
