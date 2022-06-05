using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Mmorpg.Enums;
using Mmorpg.Shared.Data;
using Swordfish.Library.Types;

namespace MMORPG.Shared.Util
{
    public static class Characters
    {
        private static ConcurrentDictionary<int, CharacterRace> CharacterRaces;
        private static ConcurrentDictionary<int, CharacterClass> CharacterClasses;
        private static ConcurrentDictionary<int, Bitmask> CharacterRaceClassCombinations;

        static Characters()
        {
            CharacterRaces = new ConcurrentDictionary<int, CharacterRace>();
            CharacterClasses = new ConcurrentDictionary<int, CharacterClass>();
            CharacterRaceClassCombinations = new ConcurrentDictionary<int, Bitmask>();
        }

        public static IEnumerable<CharacterRace> Races {
            get => CharacterRaces.Values;
            set {
                CharacterRaces.Clear();
                foreach (CharacterRace characterRace in value)
                    CharacterRaces.TryAdd(characterRace.ID, characterRace);
            }
        }

        public static IEnumerable<CharacterClass> Classes {
            get => CharacterClasses.Values;
            set {
                CharacterClasses.Clear();
                foreach (CharacterClass characterClass in value)
                    CharacterClasses.TryAdd(characterClass.ID, characterClass);
            }
        }

        public static IEnumerable<Bitmask> RaceClassCombinations {
            get => CharacterRaceClassCombinations.Values;
            set {
                CharacterRaceClassCombinations.Clear();
                int raceIndex = 1;
                foreach (Bitmask classMask in value)
                {
                    CharacterRaceClassCombinations.TryAdd(raceIndex, classMask);
                    raceIndex++;
                }
            }
        }

        public static CharacterRace GetRace(int id)
        {
            if (!CharacterRaces.TryGetValue(id, out CharacterRace value))
                throw new ArgumentNullException($"Invalid race id {id}");
            
            return value;
        }

        public static CharacterClass GetClass(int id)
        {
            if (!CharacterClasses.TryGetValue(id, out CharacterClass value))
                throw new ArgumentNullException($"Invalid class id {id}");

            return value;
        }

        public static IEnumerable<CharacterClass> GetClassesForRace(CharacterRace race) => GetClassesForRace(race.ID);

        public static IEnumerable<CharacterClass> GetClassesForRace(int raceID)
        {
            List<CharacterClass> classes = null;
            if (CharacterRaceClassCombinations.TryGetValue(raceID, out Bitmask classMask))
            {
                classes = new List<CharacterClass>();
                for (int i = 0; i < classMask.Length; i++)
                    if (classMask.Get(i))
                        classes.Add(CharacterClasses[i+1]);
            }
            
            return classes ?? CharacterClasses.Values;
        }

        public static CreateCharacterFlags ValidateRaceClassCombination(CharacterRace chosenRace, CharacterClass chosenClass)
            => ValidateRaceClassCombination(chosenRace.ID, chosenClass.ID);

        public static CreateCharacterFlags ValidateRaceClassCombination(int chosenRace, int chosenClass)
        {
            IEnumerable<int> validClasses = GetClassesForRace(chosenRace).Select(validClass => validClass.ID);
            if (validClasses.Count() == 0 || !validClasses.Contains(chosenClass))
                return CreateCharacterFlags.InvalidCombination;
            
            return CreateCharacterFlags.None;
        }
    }
}
