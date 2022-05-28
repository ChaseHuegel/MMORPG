using System.Collections.Concurrent;

using Swordfish.Integrations.SQL;
using Swordfish.Library.Extensions;
using Swordfish.Library.Types;
using Mmorpg.Data;
using Mmorpg.Enums;
using Mmorpg.Server.Data;

namespace MMORPG.Server.Util
{
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

        public static CreateCharacterFlags ValidateAndCleanName(string name, out string cleanedName)
        {
            if (string.IsNullOrEmpty(name))
            {
                cleanedName = string.Empty;
                return CreateCharacterFlags.InvalidRequest;
            }
            
            CreateCharacterFlags flags = CreateCharacterFlags.None;

            //  Spaces are allowed within a name, however...
            //  Don't allow whitespace and split on spaces to clean the name,
            //  and expunge sequential spaces by not allowing empty entries.
            string[] nameParts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string noSpacedName = string.Concat(nameParts);

            //  Allow hypens in names. We'll expunge hyphens before doing any checks on the name.
            string expungedName = string.Concat(nameParts.SelectMany(x => x.Split('-', StringSplitOptions.RemoveEmptyEntries)));

            //  Restore spaces to the name appropriately.
            cleanedName = string.Join(' ', nameParts);

            //  Spaces should not count toward length
            if (expungedName.Length < CharacterConstants.MINIMUM_NAME_LENGTH || expungedName.Length > CharacterConstants.MAXIMUM_NAME_LENGTH)
                flags = CreateCharacterFlags.NameInvalidLength;
            
            if (!expungedName.IsAlphabetic())
                flags = CreateCharacterFlags.NameInvalidFormat;

            if (VerifyName(cleanedName)) 
                flags = CreateCharacterFlags.NameTaken;
            
            if (IsNameBlacklisted(cleanedName))
                flags = CreateCharacterFlags.NameInvalidFormat;

            return flags;
        }

        private static bool VerifyName(string name)
        {
            return Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                .GetRecord("characters", "name", name).Exists();
        }

        private static bool IsNameBlacklisted(string name)
        {
            //  TODO read in list of blacklisted names and compare against it
            return false;
        }

        public static Character[] GetCharacterList(string username)
        {
            List<string> guidStrings = new List<string>();

            //  Collect all GUIDs from the user's character list
            //  TODO change this into a single query
            for (int i = 1; i <= CharacterConstants.CHARACTER_SLOT_COUNT; i++)
            {
                QueryResult guidResult = Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .GetRecord("characterLists", $"character{i}", "username", username);
                
                if (guidResult.Exists())
                {
                    string guidString = guidResult.Table.Rows[0][0].ToString();
                    guidStrings.Add(guidString);
                }
            }
            
            //  Collect all character names from respective GUIDs
            List<Character> characters = new List<Character>();
            foreach (string guidString in guidStrings)
            {
                if (!string.IsNullOrEmpty(guidString))
                {
                    QueryResult namesResult = Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                        .GetRecord("characters", "*", "guid", guidString);

                    Character character = new Character {
                        Guid = Guid.Parse(guidString),
                        Name = namesResult.Table.Rows[0]["name"].ToString(),
                        Race = (int)namesResult.Table.Rows[0]["race"],
                        Class = (int)namesResult.Table.Rows[0]["class"],
                    };
                        
                    characters.Add(character);
                }
                else
                {
                    characters.Add(null);
                }
            }

            return characters.ToArray();
        }

        public static void DeleteCharacter(string username, int slot)
        {
            QueryResult guidResult = Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .GetRecord("characterLists", $"character{slot}", "username", username);
            string guidString = guidResult.Table.Rows[0][$"character{slot}"].ToString();

            //  Clear the slot in the user's character list
            Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .Update("characterLists")
                    .Set($"character{slot}")
                    .EqualTo(null)
                    .Where("username")
                    .Equals(username)
                    .Execute();
            
            //  We don't actually delete the character data, but mark it as deleted on 'dd/mm/yyyy'
            //  Since it has been removed from the character list, it isn't accessible to users anymore.
            Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .Update("characters")
                    .Set("deleted")
                    .EqualTo(DateTime.Now.ToString("d"))
                    .Where("guid")
                    .Equals(guidString)
                    .Execute();
        }

        public static void CreateCharacter(string name, DynamicEnumValue chosenRace, DynamicEnumValue chosenClass, string username, int slot)
        {            
            //  Create character data
            string guidString = Guid.NewGuid().ToString();

            //  Insert the character into the user's character list at the slot
            Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .Update("characterLists")
                    .Set($"character{slot}")
                    .EqualTo(guidString)
                    .Where("username")
                    .Equals(username)
                    .Execute();

            //  Insert the character data
            Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .InsertInto("characters")
                    .Columns("guid", "name", "race", "class")
                    .Values(guidString, name, chosenRace.ID.ToString(), chosenClass.ID.ToString())
                    .Execute();
        }

        public static bool TryGetOpenSlot(string username, out int slot)
        {
            //  Verify the account has a character list and create one if not.
            if (!Database.Query("mmorpg", "127.0.0.1", 1433, 5).GetRecord("characterLists", "username", username).Exists())
            {
                Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .InsertInto("characterLists")
                    .Columns("username")
                    .Values(username)
                    .Execute();
            }

            //  TODO change this into a single query
            for (int i = 1; i <= CharacterConstants.CHARACTER_SLOT_COUNT; i++)
            {
                if (IsSlotOpen(username, i))
                {
                    slot = i;
                    return true;
                }
            }

            slot = -1;
            return false;
        }

        public static bool IsSlotOpen(string username, int slot)
        {
            return Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                .Select($"character{slot}").From("characterLists").Where("username").Equals(username)
                .GetResult().Table.Columns[0].Table.Rows[0].IsNull(0);
        }
    }
}
