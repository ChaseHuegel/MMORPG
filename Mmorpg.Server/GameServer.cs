using System.Collections.Concurrent;
using System.Net;

using Mmorpg.Server.Control;
using Mmorpg.Server.Data;
using Mmorpg.Shared.Data;

using MMORPG.Shared.Util;

using Swordfish.Integrations.SQL;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Handlers;
using Swordfish.Library.Networking.Packets;
using Swordfish.Library.Types;
using Swordfish.Library.Util;

namespace Mmorpg.Server
{
    public class GameServer : NetServer
    {
        public static GameServer Instance;
        public WorldView WorldView;

        public ConcurrentDictionary<EndPoint, Character> Players = new ConcurrentDictionary<EndPoint, Character>();
        public ConcurrentDictionary<EndPoint, string> Logins = new ConcurrentDictionary<EndPoint, string>();

        public GameServer(NetControllerSettings settings) : base(settings)
        {
            Instance = this;

            HandshakeHandler.ValidateHandshakeCallback = ValidateHandshake;
            HandshakePacket.ValidationSignature = "Ekahsdnah";

            InitializeData();
            WorldView = new WorldView(this);
        }

        public void InitializeData()
        {
            //  Collect all races
            List<CharacterRace> races = new List<CharacterRace>();
            QueryResult racesResult = Database.Query("mmorpg", "127.0.0.1", 1433, 5).Select("*").From("races").GetResult();
            for (int i = 0; i < racesResult.Table.Rows.Count; i++)
            {
                races.Add(new CharacterRace
                {
                    ID = (int)racesResult.Table.Rows[i][0],
                    Name = racesResult.Table.Rows[i][1].ToString(),
                    Brief = racesResult.Table.Rows[i][2].ToString(),
                    Description = racesResult.Table.Rows[i][3].ToString(),
                    //  TODO replace Bitmask usage with a new Bitflags class in Swordfish.
                    //  TODO The system BitArray doesn't work on it's own since it doesn't convert cleanly.
                    //  TODO It could be the basis of a Bitflags inheritor.
                    //  A bitmask is an inverse operation; 'off' is considered true so we invert the value to behave like flags.
                    ClassFlags = ~(int)racesResult.Table.Rows[i][4],
                });
            }
            Characters.Races = races;
            Console.WriteLine($"Races: {string.Join(", ", races.Select(x => x.Name))}");

            //  Collect all classes
            List<CharacterClass> classes = new List<CharacterClass>();
            QueryResult classesResult = Database.Query("mmorpg", "127.0.0.1", 1433, 5).Select("*").From("classes").GetResult();
            for (int i = 0; i < classesResult.Table.Rows.Count; i++)
            {
                classes.Add(new CharacterClass
                {
                    ID = (int)classesResult.Table.Rows[i][0],
                    Name = classesResult.Table.Rows[i][1].ToString(),
                    AbilityFlags = (long)classesResult.Table.Rows[i][2],
                    ResourceFlags = (int)classesResult.Table.Rows[i][3],
                    ArmorProficiencyFlags = (long)classesResult.Table.Rows[i][4],
                    WeaponProficiencyFlags = (long)classesResult.Table.Rows[i][5],
                    OtherProficiencyFlags = (long)classesResult.Table.Rows[i][6],
                    Brief = classesResult.Table.Rows[i][7].ToString(),
                    Description = classesResult.Table.Rows[i][8].ToString(),
                });
            }
            Characters.Classes = classes;
            Console.WriteLine($"Classes: {string.Join(", ", classes.Select(x => x.Name))}");

            //  Collect all race-class combinations
            List<Bitmask> combinations = new List<Bitmask>();
            foreach (CharacterRace race in races)
                combinations.Add(race.ClassFlags);

            Characters.RaceClassCombinations = combinations;
        }

        public void Tick(float deltaTime)
        {
            WorldView.Tick(deltaTime);
        }

        public bool ValidateHandshake(EndPoint endPoint)
        {
            return Logins.ContainsKey(endPoint);
        }

        public override void OnSessionStarted(object sender, NetEventArgs e)
        {
            Players.TryGetValue(e.EndPoint, out Character character);
            WorldView.AddPlayer(character, e.Session);

            Console.WriteLine($"[{e.Session}] joined the game world as [{character.Name}].");
        }

        public override void OnSessionEnded(object sender, NetEventArgs e)
        {
            Logins.TryRemove(e.EndPoint, out string username);
            Players.TryRemove(e.EndPoint, out Character character);
            WorldView.RemovePlayer(e.Session);
            Console.WriteLine($"[{e.Session}] disconnected, logged out [{username}] as [{character.Name}].");
        }
    }
}
