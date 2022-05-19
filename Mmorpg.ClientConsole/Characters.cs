using System.Collections.Concurrent;

using Swordfish.Library.Types;
using Mmorpg.Data;
using Mmorpg.Enums;

namespace MMORPG.Server.Util
{
    //  TODO this should be populated with data from the server
    public static class Characters
    {
        private static ConcurrentDictionary<DynamicEnumValue, DynamicEnumValue[]> s_RaceClassCombinations;
        private static ConcurrentDictionary<DynamicEnumValue, DynamicEnumValue[]> RaceClassCombinations = s_RaceClassCombinations ?? (s_RaceClassCombinations = LoadRaceClassCombinations());

        private static ConcurrentDictionary<DynamicEnumValue, DynamicEnumValue[]> LoadRaceClassCombinations()
        {
            //  TODO populate race/class combinations from database on the server and from data store on client
            ConcurrentDictionary<DynamicEnumValue, DynamicEnumValue[]> combinations = new();
            combinations.TryAdd(CharacterRaces.Get("Human"),
                GetAllClasses());

            combinations.TryAdd(CharacterRaces.Get("Elf"), new DynamicEnumValue[3] {
                CharacterClasses.Get("Fighter"),
                CharacterClasses.Get("Rogue"),
                CharacterClasses.Get("Mage")});
            
            combinations.TryAdd(CharacterRaces.Get("Dwarf"), new DynamicEnumValue[2] {
                CharacterClasses.Get("Fighter"),
                CharacterClasses.Get("Cleric")});

            return combinations;
        }

        public static DynamicEnumValue[] GetAllRaces()
        {
            //  TODO populate classes from database and make it a dictionary
            return CharacterRaces.GetValues();
        }

        public static DynamicEnumValue[] GetAllClasses()
        {
            //  TODO populate classes from database and make it a dictionary
            return CharacterClasses.GetValues();
        }

        public static DynamicEnumValue[] GetValidClasses(DynamicEnumValue race)
        {
            if (RaceClassCombinations.TryGetValue(race, out DynamicEnumValue[] classes))
                return classes;
            
            return new DynamicEnumValue[0];
        }

        public static DynamicEnumValue GetCharacterClass(int id)
        {
            return CharacterClasses.Get(id);
        }

        public static DynamicEnumValue GetCharacterRace(int id)
        {
            return CharacterRaces.Get(id);
        }

        public static CreateCharacterFlags ValidateRaceClass(DynamicEnumValue chosenRace, DynamicEnumValue chosenClass)
        {
            DynamicEnumValue[] validClasses = GetValidClasses(chosenRace);
            if (validClasses.Length == 0 || !validClasses.Contains(chosenClass))
                return CreateCharacterFlags.InvalidCombo;
            
            return CreateCharacterFlags.None;
        }
    }
}
