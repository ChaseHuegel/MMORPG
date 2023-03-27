namespace MMO.Bridge.Types;

[Flags]
public enum ChatChannel
{
    None = 0,
    Tell = 1 << 0,
    Emote = 1 << 1,
    Local = 1 << 2,
    Yell = 1 << 3,
    General = 1 << 4,
    Trade = 1 << 5,
    Help = 1 << 6,
    Group = 1 << 7,
    Raid = 1 << 8,
    Guild = 1 << 9,
    System = 1 << 10,
    Notify = 1 << 11,
    Combat = 1 << 12,
    Title = 1 << 13,
    Subtitle = 1 << 14,
    Debug = 1 << 15,
    All = ~0
}
