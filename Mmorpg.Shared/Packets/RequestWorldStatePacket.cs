using System;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true)]
    public struct RequestWorldStatePacket : ISerializedPacket
    {
        
    }
}
