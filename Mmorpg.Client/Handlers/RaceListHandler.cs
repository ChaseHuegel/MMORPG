using System;
using System.Collections.Generic;
using System.Linq;

using Mmorpg.Packets;
using Mmorpg.Shared.Data;

using MMORPG.Shared.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Client.Handlers
{
    public static class RaceListHandler
    {
        [ClientPacketHandler]
        public static void OnRaceListClient(NetClient client, RaceListPacket packet, NetEventArgs e)
        {
            List<CharacterRace> races = new List<CharacterRace>();

            Console.WriteLine($"Race List");
            for (int i = 0; i < packet.RaceNames.Length; i++)
            {
                Console.WriteLine($"[{i+1}] {packet.RaceNames[i]}");

                races.Add(new CharacterRace() {
                    ID = i+1,
                    Name = packet.RaceNames[i]
                });
            }
            
            Characters.Races = races; 
        }
    }
}
