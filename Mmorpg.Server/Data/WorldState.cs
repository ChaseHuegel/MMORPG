using System.Collections.Concurrent;
using Mmorpg.Data;

namespace Mmorpg.Server.Data
{
    //  TODO this should be loaded from data
    //  Hardcoded for testing
    public class WorldState
    {
        public ConcurrentDictionary<int, LivingEntity> Players = new ConcurrentDictionary<int, LivingEntity>();
        public ConcurrentDictionary<int, NPC> NPCs = new ConcurrentDictionary<int, NPC>();

        public WorldState()
        {
            Random random = new Random();

            AddNPC("Darin Blackbriar", "Guard Captain", 5, 5);
            AddNPC("Mira Hartsly", string.Empty, 1, 3);
            AddNPC("Haldo Brockhill", "Merchant", -10, 0);

            for (int i = 0; i < 10; i++)
                AddNPC("Bandit", string.Empty, 50, -50).Wander(true);
        }

        private int CreateID() => 10000 + NPCs.Count;

        private NPC AddNPC(string name, string title, float x, float z)
        {
            int id = CreateID();
            NPC npc = new NPC {
                Name = name,
                Title = title,
                ID = id,
                X = x,
                Z = z
            };

            NPCs.TryAdd(id, npc);
            return npc;
        }
    }
}
