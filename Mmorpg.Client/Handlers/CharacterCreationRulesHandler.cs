using System;
using System.Collections.Generic;
using System.Linq;
using Mmorpg.Shared.Data;
using Mmorpg.Shared.Packets;

using MMORPG.Shared.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Types;

namespace Mmorpg.Client.Handlers
{
    public static class CharacterCreationRulesHandler
    {
        [ClientPacketHandler]
        public static void OnCharacterCreationRulesClient(NetClient client, CharacterCreationRulesPacket packet, NetEventArgs e)
        {    
            List<CharacterRace> races = new List<CharacterRace>();
            List<CharacterClass> classes = new List<CharacterClass>();
            List<Bitmask> combinations = new List<Bitmask>();

            for (int i = 0; i < packet.RaceNames.Length; i++)
            {
                races.Add(new CharacterRace() {
                    ID = i+1,
                    Name = packet.RaceNames[i]
                });
            }
            Console.WriteLine($"Race List: {string.Join(", ", races.Select(r => r.Name))}");

            for (int i = 0; i < packet.ClassNames.Length; i++)
            {
                classes.Add(new CharacterClass() {
                    ID = i+1,
                    Name = packet.ClassNames[i]
                });
            }
            Console.WriteLine($"Class List: {string.Join(", ", classes.Select(c => c.Name))}");

            for (int i = 0; i < races.Count; i++)
            {
                Bitmask mask = packet.RaceClassMask[i];
                combinations.Add(mask);
                
                List<string> validClasses = new List<string>();
                for (int n = 0; n < classes.Count; n++)
                    if (mask.Get(n))
                        validClasses.Add(classes[n].Name);
                
                Console.WriteLine($"[{races[i].Name}] allowed classes: {string.Join(", ", validClasses)}");
            }
            
            Characters.Races = races;
            Characters.Classes = classes;
            Characters.RaceClassCombinations = combinations;
        }
    }
}
