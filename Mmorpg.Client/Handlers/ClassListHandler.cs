using System;
using System.Collections.Generic;

using Mmorpg.Packets;
using Mmorpg.Shared.Data;

using MMORPG.Shared.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Client.Handlers
{
    public static class ClassListHandler
    {
        [ClientPacketHandler]
        public static void OnClassListClient(NetClient client, ClassListPacket packet, NetEventArgs e)
        {    
            List<CharacterClass> classes = new List<CharacterClass>();

            Console.WriteLine($"Class List");
            for (int i = 0; i < packet.ClassNames.Length; i++)
            {
                Console.WriteLine($"[{i+1}] {packet.ClassNames[i]}");

                classes.Add(new CharacterClass() {
                    ID = i+1,
                    Name = packet.ClassNames[i]
                });
            }
            
            Characters.Classes = classes; 
        }
    }
}
