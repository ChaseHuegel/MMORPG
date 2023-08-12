namespace MMO.Bridge.Models;

public class Character
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int CurrentXP { get; set; }
    public CharacterClass Class { get; set; }
    public CharacterRace Race { get; set; }
    public Dictionary<Skill, SkillData> Skills { get; set; }
}
