namespace MMO.Bridge.Models;

public struct CharacterCreationRules
{
    public int MinNameLength { get; set; }
    public int MaxNameLength { get; set; }

    public string AllowedNameChars { get; set; }

    public CharacterRace[] Races { get; set; }

    public CharacterClass[] Classes { get; set; }

    public Dictionary<int, int[]> AllowedCombinations { get; set; }
}