using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;
using System;

namespace Mmorpg.Client.Handlers
{
    public static class CharacterListHandler
    {
        [ClientPacketHandler]
        public static void OnCharacterListPacketClient(NetClient client, CharacterListPacket packet, NetEventArgs e)
        {
            AccountFlags flags = (AccountFlags)packet.Flags;

            if (flags == AccountFlags.None)
            {
                Console.WriteLine($"Character List");
                for (int i = 0; i < packet.CharacterNames.Length; i++)
                    Console.WriteLine($"[{i+1}] {packet.CharacterNames[i]}");
            }
            else
            {
                Console.WriteLine($"Failed to retrieve character list: {flags}");
            }
        }
    }
}
